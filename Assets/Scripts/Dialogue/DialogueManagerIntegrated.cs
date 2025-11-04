using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class DialogueManagerIntegrated : MonoBehaviour
{
    public static DialogueManagerIntegrated Instance { get; private set; }
    public static Translator translator;

   
    [Header("Typewriter")]
    [SerializeField] private float charsPerSecond = 45f;      // typing speed
    [SerializeField] private KeyCode advanceKey = KeyCode.Space;
    [SerializeField] private KeyCode altAdvanceKey = KeyCode.Return;
    [SerializeField] private bool clickAdvances = true;

    // --- Journal pause state ---
    private bool _pausedByJournal = false;
    private readonly List<UnityEngine.UI.Button> _choiceButtons = new();

    private bool isTyping;
    private Coroutine typingRoutine;
    private Coroutine autoNextRoutine;

    // Track current node’s choice buttons/labels so journal can edit if needed
    private readonly List<TMP_Text> _choiceLabels = new List<TMP_Text>();

    
    [SerializeField] private bool _mutateNodeChoiceTextOnSet = true;

    public bool isFinished { get; private set; } = false;

    private Conversation active;
    private DNode current;

    void Awake()
    {
        Instance = this;

        // Ensure we have a Translator on the same GameObject
        translator = GetComponent<Translator>();
        if (translator == null) translator = gameObject.AddComponent<Translator>();
    }

    void Update()
    {
        if (_pausedByJournal) return; 
        if (active == null) return;

        bool advancePressed =
            Input.GetKeyDown(advanceKey) ||
            Input.GetKeyDown(altAdvanceKey) ||
            (clickAdvances && Input.GetMouseButtonDown(0));

        if (!advancePressed) return;

        // If still typing, finish current line instantly
        if (isTyping)
        {
            FinishTypingNow();
            return;
        }

        // If choices exist, player must click a choice; don't auto-advance
        if (current != null && current.choices != null && current.choices.Length > 0)
            return;

        // Linear advance (or end)
        if (current != null)
        {
            if (autoNextRoutine != null)
            {
                StopCoroutine(autoNextRoutine);
                autoNextRoutine = null;
            }

            if (current.nextIfNoChoices != null) ShowNode(current.nextIfNoChoices);
            else EndConversation();
        }
    }

    public void StartConversation(Conversation convo)
    {
        if (convo == null || convo.entry == null) return;
        isFinished = false;
        active = convo;
        ShowNode(convo.entry);
    }

    private void ShowNode(DNode node)
    {
        if (node == null)
        {
            Debug.LogError("ShowNode called with null node.");
            return;
        }

        current = node;

        // Fire per-node inspector event
        node.onEnter?.Invoke();

        // UI
        var ui = DialogueController.Instance;
        if (ui == null)
        {
            Debug.LogError("DialogueController.Instance is null");
            return;
        }

        ui.ShowDialogueUI(true);

        bool encode = (CipherDecode.instance != null && CipherDecode.instance.encoding);

        // Encode speaker name & line
        string nameOut = encode ? translator.Translate(node.speakerName) : node.speakerName;
        string lineOut = encode ? translator.Translate(node.speakerLine) : node.speakerLine;

        // Push name + portrait
        ui.SetCharInfo(nameOut, node.portrait);

        // Clear prior buttons, reset cache
        ui.ClearChoices();
        _choiceLabels.Clear();
        _choiceButtons.Clear();

        // Prepare and start typewriter for the body text
        var body = ui.dialogueText; // ensure DialogueController exposes this TMP_Text
        if (body == null)
        {
            Debug.LogError("DialogueController.dialogueText is not assigned.");
            return;
        }

        // Stop any previous routines
        if (typingRoutine != null) { StopCoroutine(typingRoutine); typingRoutine = null; }
        if (autoNextRoutine != null) { StopCoroutine(autoNextRoutine); autoNextRoutine = null; }

        typingRoutine = StartCoroutine(TypeLine(body, lineOut, node));
    }

    // Call when opening the Journal 
    public void PauseForJournal(bool hideChoices = false, bool finishLine = true)
    {
        if (_pausedByJournal) return;
        _pausedByJournal = true;


        if (autoNextRoutine != null) { StopCoroutine(autoNextRoutine); autoNextRoutine = null; }
        if (finishLine) FinishTypingNow();

        // disable choice buttons so nothing can be clicked behind the journal
        foreach (var b in _choiceButtons) if (b) b.interactable = false;
    }
    
    
    public void PauseForJournal()
    {
        PauseForJournal(true, true);   // hideChoices = true, finishLine = true
    }


    public void PauseForJournalFinish(bool finishLine)
    {
        PauseForJournal(true, finishLine);
    }

    // Call when closing the Journal
    public void ResumeFromJournal()
    {
        if (!_pausedByJournal) return;
        _pausedByJournal = false;

        // re-enable buttons
        foreach (var b in _choiceButtons) if (b) b.interactable = true;

        RefreshAllTexts();
    }

    private IEnumerator TypeLine(TMP_Text label, string fullText, DNode node)
    {
        isTyping = true;

        label.text = fullText;
        label.ForceMeshUpdate();
        int total = Mathf.Max(0, label.textInfo.characterCount);

        // Hide all characters, reveal over time
        label.maxVisibleCharacters = 0;

        if (charsPerSecond <= 0f)
        {
            label.maxVisibleCharacters = 999999;
        }
        else
        {
            float t = 0f;
            while (label.maxVisibleCharacters < total)
            {
                t += Time.unscaledDeltaTime * charsPerSecond;
                int visible = Mathf.Clamp(Mathf.FloorToInt(t), 0, total);
                label.maxVisibleCharacters = visible;
                yield return null;
            }
            label.maxVisibleCharacters = 999999;
        }

        isTyping = false;
        typingRoutine = null;

        // After typing finishes:
        if (node.choices != null && node.choices.Length > 0)
        {
            BuildChoices(node);
        }
        else if (node.autoProgress && node.nextIfNoChoices != null)
        {
            // Start auto-next (click/space will cancel and advance immediately)
            autoNextRoutine = StartCoroutine(AutoNext(node.nextIfNoChoices, node.autoDelay));
        }
        else
        {
            // Also provide a Continue button just in case
            var go = DialogueController.Instance.CreateChoiceButton("Continue", () =>
            {
                if (node.nextIfNoChoices != null) ShowNode(node.nextIfNoChoices);
                else EndConversation();
            });
            if (go != null)
            {
                var lbl = go.GetComponentInChildren<TMP_Text>(true);
                if (lbl != null) _choiceLabels.Add(lbl);
            }
        }
    }

    private void BuildChoices(DNode node)
    {
        var ui = DialogueController.Instance;
        bool encode = (CipherDecode.instance != null && CipherDecode.instance.encoding);

        foreach (var c in node.choices)
        {
            var choiceCopy = c; // capture for closure
            GameObject choiceButtonGO = ui.CreateChoiceButton(
                encode ? translator.Translate(choiceCopy.choiceText) : choiceCopy.choiceText,
                () => OnChoiceSelected(choiceCopy)
            );

            if (choiceButtonGO != null)
            {
                var label = choiceButtonGO.GetComponentInChildren<TMP_Text>(true);
                if (label != null) _choiceLabels.Add(label);
                var btn = choiceButtonGO.GetComponent<UnityEngine.UI.Button>();
                if (btn) _choiceButtons.Add(btn);
            }
        }
    }




    // Jump into any node (line)
    public void JumpToNode(DNode node)
    {
        if (!node) return;
        isFinished = false;

        if (typingRoutine != null) { StopCoroutine(typingRoutine); typingRoutine = null; }
        if (autoNextRoutine != null) { StopCoroutine(autoNextRoutine); autoNextRoutine = null; }

        ShowNode(node);
    }

    //delayed version
    public void JumpToNodeDelayed(DNode node, float delay)
    {
        if (!node) return;
        StartCoroutine(_JumpDelayed(node, delay));
    }
    private IEnumerator _JumpDelayed(DNode node, float delay)
    {
        yield return new WaitForSeconds(Mathf.Max(0f, delay));
        JumpToNode(node);
    }

    public void StartConversationAt(Conversation convo, DNode startNode)
    {
        if (!convo) return;
        isFinished = false;
        active = convo;

        if (typingRoutine != null) { StopCoroutine(typingRoutine); typingRoutine = null; }
        if (autoNextRoutine != null) { StopCoroutine(autoNextRoutine); autoNextRoutine = null; }

        ShowNode(startNode ? startNode : convo.entry);
    }

    private void FinishTypingNow()
    {
        isTyping = false;
        if (typingRoutine != null)
        {
            StopCoroutine(typingRoutine);
            typingRoutine = null;
        }

        var body = DialogueController.Instance?.dialogueText;
        if (body != null) body.maxVisibleCharacters = 999999;
    }

    private void OnChoiceSelected(Choice c)
    {
        var ui = DialogueController.Instance;
        if (ui == null) { EndConversation(); return; }

        // Cancel pending auto-next
        if (autoNextRoutine != null)
        {
            StopCoroutine(autoNextRoutine);
            autoNextRoutine = null;
        }

        if (c == null) { EndConversation(); return; }

        if (c.isTriviaQuestion)
        {
            if (c.isCorrect)
            {
                ui.ClearChoices();
                ui.SetDialogueText("Correct!");

                if (!string.IsNullOrEmpty(c.loadSceneOnSelect))
                {
                    isFinished = true;
                    StopAllCoroutines();
                    StartCoroutine(AutoLoadScene(c.loadSceneOnSelect, 1.0f));
                    return;
                }
            }
            else
            {
                ui.ClearChoices();
                ui.SetDialogueText("Incorrect.");
            }
        }
        else
        {
            // run unity event
            c.onSelected.Invoke();
        }

        if (c.next != null) ShowNode(c.next);
        else EndConversation();
    }

    private IEnumerator AutoLoadScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(Mathf.Max(0f, delay));
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator AutoNext(DNode next, float delay)
    {
        float t = Mathf.Max(0f, delay);
        while (t > 0f)
        {
            bool advancePressed =
                Input.GetKeyDown(advanceKey) ||
                Input.GetKeyDown(altAdvanceKey) ||
                (clickAdvances && Input.GetMouseButtonDown(0));

            if (advancePressed) break;

            t -= Time.unscaledDeltaTime;
            yield return null;
        }

        autoNextRoutine = null;
        ShowNode(next);
    }

    public void EndConversation()
    {
        var ui = DialogueController.Instance;
        if (ui != null) ui.ShowDialogueUI(false);

        // stop any pending routines
        if (typingRoutine != null) { StopCoroutine(typingRoutine); typingRoutine = null; }
        if (autoNextRoutine != null) { StopCoroutine(autoNextRoutine); autoNextRoutine = null; }

        active = null;
        current = null;
        isTyping = false;
        isFinished = true;
        StopAllCoroutines();
    }

    // -------------------------
    // Journal-facing API
    // -------------------------

    //Change visible choice text by index (0-based). Optionally encode & mutate node data
    public void SetChoiceText(int index, string newText, bool encode = true)
    {
        if (current == null) return;
        if (index < 0 || index >= _choiceLabels.Count) return;

        if (_mutateNodeChoiceTextOnSet &&
            current.choices != null &&
            index < current.choices.Length &&
            current.choices[index] != null)
        {
            current.choices[index].choiceText = newText;
        }

        bool doEncode = encode && (CipherDecode.instance != null && CipherDecode.instance.encoding);
        string final = doEncode ? translator.Translate(newText) : newText;

        _choiceLabels[index].text = final;
    }

    //Replace all visible choice texts
    public void SetAllChoiceTexts(IList<string> newTexts, bool encode = true)
    {
        if (current == null || newTexts == null) return;
        int count = Mathf.Min(_choiceLabels.Count, newTexts.Count);
        for (int i = 0; i < count; i++)
            SetChoiceText(i, newTexts[i], encode);
    }

    //Re-translate visible choice labels using current cipher
    public void RefreshChoiceTexts()
    {
        if (current == null || current.choices == null) return;

        bool encode = (CipherDecode.instance != null && CipherDecode.instance.encoding);

        for (int i = 0; i < _choiceLabels.Count && i < current.choices.Length; i++)
        {
            string raw = current.choices[i]?.choiceText ?? "";
            string final = encode ? translator.Translate(raw) : raw;
            _choiceLabels[i].text = final;
        }
    }

    //Set current node's body text. Optionally encode & mutate node data
    public void SetBodyText(string newText, bool encode = true, bool mutateNode = true)
    {
        var ui = DialogueController.Instance;
        if (ui == null) return;

        if (mutateNode && current != null)
            current.speakerLine = newText;

        bool doEncode = encode && (CipherDecode.instance != null && CipherDecode.instance.encoding);
        string final = doEncode ? translator.Translate(newText) : newText;

        ui.SetDialogueText(final);

        // ensure full visibility if in middle of typing
        FinishTypingNow();
    }

    //Re-apply encoding to the current node's body text
    public void RefreshBodyText()
    {
        var ui = DialogueController.Instance;
        if (ui == null || current == null) return;

        string raw = current.speakerLine ?? "";
        bool encode = (CipherDecode.instance != null && CipherDecode.instance.encoding);
        string final = encode ? translator.Translate(raw) : raw;

        ui.SetDialogueText(final);
        FinishTypingNow();
    }

   
    public void SetSpeakerName(string newName, bool encode = true, bool mutateNode = true)
    {
        var ui = DialogueController.Instance;
        if (ui == null) return;

        if (mutateNode && current != null)
            current.speakerName = newName;

        bool doEncode = encode && (CipherDecode.instance != null && CipherDecode.instance.encoding);
        string final = doEncode ? translator.Translate(newName) : newName;

        ui.SetCharInfo(final, current != null ? current.portrait : null);
    }

    
    public void RefreshSpeakerName()
    {
        var ui = DialogueController.Instance;
        if (ui == null || current == null) return;

        string raw = current.speakerName ?? "";
        bool encode = (CipherDecode.instance != null && CipherDecode.instance.encoding);
        string final = encode ? translator.Translate(raw) : raw;

        ui.SetCharInfo(final, current.portrait);
    }

    //refresh all
    public void RefreshAllTexts()
    {
        RefreshBodyText();
        RefreshChoiceTexts();
    }
}
