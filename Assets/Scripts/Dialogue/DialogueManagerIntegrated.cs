using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class DialogueManagerIntegrated : MonoBehaviour
{
    public static DialogueManagerIntegrated Instance { get; private set; }
    public static Translator translator;

    // Track current node’s choice buttons/labels so others can edit them
    private readonly List<TMP_Text> _choiceLabels = new List<TMP_Text>();

    // If true, also mutate the underlying node data when SetChoiceText is called
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

        // Encode speaker name and line
        string nameOut = encode ? translator.Translate(node.speakerName) : node.speakerName;
        string lineOut = encode ? translator.Translate(node.speakerLine)  : node.speakerLine;

        // Push to UI
        ui.SetCharInfo(nameOut, node.portrait);
        ui.SetDialogueText(lineOut);


        ui.ClearChoices();
        _choiceLabels.Clear();

        // Branching: create choice buttons once
        if (node.choices != null && node.choices.Length > 0)
        {
            foreach (var choice in node.choices)
            {
                var choiceCopy = choice; 

                GameObject choiceButtonGO = ui.CreateChoiceButton(
                    encode ? translator.Translate(choiceCopy.choiceText) : choiceCopy.choiceText,
                    () => OnChoiceSelected(choiceCopy)
                );

                // Cache label for live editing via Journal
                if (choiceButtonGO != null)
                {
                    var label = choiceButtonGO.GetComponentInChildren<TMP_Text>(true);
                    if (label != null) _choiceLabels.Add(label);
                }
            }
            return;
        }

        // Linear: auto-advance or show Continue
        if (node.autoProgress && node.nextIfNoChoices != null)
        {
            StopAllCoroutines();
            StartCoroutine(AutoNext(node.nextIfNoChoices, node.autoDelay));
            return;
        }

        GameObject continueButtonGO = ui.CreateChoiceButton("Continue", () =>
        {
            if (node.nextIfNoChoices != null) ShowNode(node.nextIfNoChoices);
            else EndConversation();
        });

        if (continueButtonGO != null)
        {
            var label = continueButtonGO.GetComponentInChildren<TMP_Text>(true);
            if (label != null) _choiceLabels.Add(label);
        }
    }

    private void OnChoiceSelected(Choice c)
    {
        if (c == null) { EndConversation(); return; }

        var ui = DialogueController.Instance;
        if (ui == null) { EndConversation(); return; }

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
        yield return new WaitForSeconds(Mathf.Max(0f, delay));
        ShowNode(next);
    }

    public void EndConversation()
    {
        var ui = DialogueController.Instance;
        if (ui != null) ui.ShowDialogueUI(false);
        active = null;
        current = null;
        StopAllCoroutines();
        isFinished = true; 
    }

    // -------------------------
    // Journal
    // -------------------------

    /// <summary>
    /// Change the displayed text of a choice by index.
    /// </summary>
    public void SetChoiceText(int index, string newText, bool encode = true)
    {
        if (current == null) return;
        if (index < 0 || index >= _choiceLabels.Count) return;

        // Optionally update underlying data so it persists during this conversation
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

    /// <summary>
    /// Replace all visible choice texts in one call.
    /// </summary>
    public void SetAllChoiceTexts(IList<string> newTexts, bool encode = true)
    {
        if (current == null || newTexts == null) return;
        int count = Mathf.Min(_choiceLabels.Count, newTexts.Count);
        for (int i = 0; i < count; i++)
            SetChoiceText(i, newTexts[i], encode);
    }

            /// <summary>
        /// Set the current node's speaker name
        /// </summary>
        public void SetSpeakerName(string newName, bool encode = true, bool mutateNode = true)
        {
            var ui = DialogueController.Instance;
            if (ui == null) return;

            if (mutateNode && current != null)
                current.speakerName = newName;

            bool doEncode = encode && (CipherDecode.instance != null && CipherDecode.instance.encoding);
            string final = doEncode ? translator.Translate(newName) : newName;

            // update name text
            ui.SetCharInfo(final, current != null ? current.portrait : null);
        }

        /// <summary>
        /// Re-apply encoding to the current node's existing speaker name.
        /// Call this after cipher mappings change.
        /// </summary>
        public void RefreshSpeakerName()
        {
            var ui = DialogueController.Instance;
            if (ui == null || current == null) return;

            string raw = current.speakerName ?? "";
            bool encode = (CipherDecode.instance != null && CipherDecode.instance.encoding);
            string final = encode ? translator.Translate(raw) : raw;

            ui.SetCharInfo(final, current.portrait);
        }


    /// <summary>
    /// Re-translate currently shown choice labels using the latest cipher state.
    /// Call after the Journal updates mappings.
    /// </summary>
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


    public void RefreshAllTexts()
    {
        RefreshSpeakerName();
        RefreshChoiceTexts();
    }
}


