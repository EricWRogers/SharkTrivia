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

        // DO NOT encode the speaker name
        string nameOut = node.speakerName;

        string lineOut = EncodeOrFallback(node.speakerLine);

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

        private void AfterTypeDone(DNode node)
        {
            // build choices if present
            if (node.choices != null && node.choices.Length > 0)
            {
                BuildChoices(node);
                return;
            }

            // or auto-advance
            if (node.autoProgress && node.nextIfNoChoices != null)
            {
                autoNextRoutine = StartCoroutine(AutoNext(node.nextIfNoChoices, node.autoDelay));
                return;
            }

            // or show a Continue button
            var go = DialogueController.Instance.CreateChoiceButton("Continue", () =>
            {
                if (node.nextIfNoChoices != null) ShowNode(node.nextIfNoChoices);
                else EndConversation();
            });
            if (go != null)
            {
                var lbl = go.GetComponentInChildren<TMP_Text>(true);
                if (lbl != null) _choiceLabels.Add(lbl);
                var btn = go.GetComponent<UnityEngine.UI.Button>();
                if (btn) _choiceButtons.Add(btn);
            }
        }

    private IEnumerator TypeLine(TMP_Text label, string fullText, DNode node)
    {
        isTyping = true;

        // set text and measure visible characters
        label.text = fullText;
        label.ForceMeshUpdate();
        int total = Mathf.Max(0, label.textInfo.characterCount);

        // If nothing to type 
        if (total == 0)
        {
            label.maxVisibleCharacters = 999999;
            isTyping = false;
            typingRoutine = null;
            AfterTypeDone(node);
            yield break; 
        }

        // Hide all characters, reveal over time
        label.maxVisibleCharacters = 0;

        if (charsPerSecond <= 0f)
        {
            // Instant reveal if speed is zero/negative
            label.maxVisibleCharacters = 999999;
        }
        else
        {
            float t = 0f;
            while (label.maxVisibleCharacters < total)
            {
                // If journal pauses, just wait
                if (_pausedByJournal)
                {
                    yield return null;
                    continue;
                }

                t += Time.unscaledDeltaTime * charsPerSecond;
                int visible = Mathf.Clamp(Mathf.FloorToInt(t), 0, total);
                label.maxVisibleCharacters = visible;
                yield return null;
            }
            label.maxVisibleCharacters = 999999;
        }

        isTyping = false;
        typingRoutine = null;

        
        AfterTypeDone(node);
        yield break;  
    }

    private void FinishTypingNow()
    {
        
        if (isTyping)
        {
            isTyping = false;

            
            if (typingRoutine != null)
            {
                StopCoroutine(typingRoutine);
                typingRoutine = null;
            }

            var body = DialogueController.Instance?.dialogueText;
            if (body != null) body.maxVisibleCharacters = 999999;

            
            if (current != null) AfterTypeDone(current);
        }
        else
        {
            
            var body = DialogueController.Instance?.dialogueText;
            if (body != null) body.maxVisibleCharacters = 999999;
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
            EncodeOrFallback(choiceCopy.choiceText),
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


    private void OnChoiceSelected(Choice c)
    {
        var ui = DialogueController.Instance;
        if (ui == null) { EndConversation(); return; }

      
        if (autoNextRoutine != null)
        {
            StopCoroutine(autoNextRoutine);
            autoNextRoutine = null;
        }

        if (c == null) { EndConversation(); return; }

        
        try
        {
            if (c.onSelected != null)
            {
                
                Debug.Log($"[Dialogue] Invoking onSelected for choice: \"{c.choiceText}\"");
                c.onSelected.Invoke();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Dialogue] onSelected threw an exception on choice \"{c.choiceText}\": {ex}");
        }

        
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

        // Continue conversation
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

        _choiceLabels[index].text = encode ? EncodeOrFallback(newText) : newText;
    }

    public void SetBodyText(string newText, bool encode = true, bool mutateNode = true)
    {
        var ui = DialogueController.Instance;
        if (ui == null) return;

        if (mutateNode && current != null) current.speakerLine = newText;

        ui.SetDialogueText(encode ? EncodeOrFallback(newText) : newText);
        FinishTypingNow();
    }

    public void RefreshBodyText()
    {
        var ui = DialogueController.Instance;
        if (ui == null || current == null) return;

        ui.SetDialogueText(EncodeOrFallback(current.speakerLine ?? ""));
        FinishTypingNow();
    }

    public void RefreshChoiceTexts()
    {
        if (current == null || current.choices == null) return;

        for (int i = 0; i < _choiceLabels.Count && i < current.choices.Length; i++)
        {
            string raw = current.choices[i]?.choiceText ?? "";
            _choiceLabels[i].text = EncodeOrFallback(raw);
        }
    }


    // Returns encoded text if encoding is ON and translator gives non-empty output,
// otherwise returns the original text.
    private string EncodeOrFallback(string raw)
    {
        bool encode = (CipherDecode.instance != null && CipherDecode.instance.encoding);
        if (!encode || string.IsNullOrEmpty(raw)) return raw;

        string enc = translator != null ? translator.Translate(raw) : raw;
        return string.IsNullOrEmpty(enc) ? raw : enc;
    }

    //Set current node's body text. Optionally encode & mutate node data
   

   
    //refresh all
    public void RefreshAllTexts()
    {
        RefreshBodyText();
        RefreshChoiceTexts();
    }
}
