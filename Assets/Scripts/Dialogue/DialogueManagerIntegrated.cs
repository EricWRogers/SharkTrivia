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

    public bool isFinished = false;
    void Awake()
    {
        Instance = this;
        gameObject.AddComponent<Translator>();
        translator = GetComponent<Translator>();
    }

    Conversation active;
    DNode current;

    public void StartConversation(Conversation convo)
    {
        if (!convo || !convo.entry) return;
        active = convo;
        ShowNode(convo.entry);

        isFinished = false;
    }

    void ShowNode(DNode node)
    {
        if (node == null) { Debug.LogError("ShowNode called with null node."); return; }

        current = node;

        // per-node inspector event
        node.onEnter?.Invoke();

        // UI on
        var ui = DialogueController.Instance;
        if (ui == null) { Debug.LogError("DialogueController.Instance is null"); return; }

        ui.ShowDialogueUI(true);
        ui.SetCharInfo(node.speakerName, node.portrait);

        // encode line with translator (guard for CipherDecode.instance)
        bool encode = (CipherDecode.instance != null && CipherDecode.instance.encoding);
        ui.SetDialogueText(encode ? translator.Translate(node.speakerLine) : node.speakerLine);

        ui.ClearChoices();
        _choiceLabels.Clear();

        // Branching – create choices once
        if (node.choices != null && node.choices.Length > 0)
        {
            foreach (var c in node.choices)
            {
                var choiceCopy = c; // capture for closure

                // Create the button (encoded or plain)
                GameObject choiceButtonGO = ui.CreateChoiceButton(
                    encode ? translator.Translate(choiceCopy.choiceText) : choiceCopy.choiceText,
                    () => OnChoiceSelected(choiceCopy)
                );

                // cache the label so we can edit it later
                if (choiceButtonGO != null)
                {
                    var label = choiceButtonGO.GetComponentInChildren<TMP_Text>(true);
                    if (label != null) _choiceLabels.Add(label);
                }
            }
            return;
        }

        // Linear
        if (node.autoProgress && node.nextIfNoChoices)
        {
            StopAllCoroutines();
            StartCoroutine(AutoNext(node.nextIfNoChoices, node.autoDelay));
            return;
        }

        // Manual continue
        GameObject continueButtonGO = ui.CreateChoiceButton("Continue", () =>
        {
            if (node.nextIfNoChoices) ShowNode(node.nextIfNoChoices);
            else EndConversation();
        });
        if (continueButtonGO != null)
        {
            var label = continueButtonGO.GetComponentInChildren<TMP_Text>(true);
            if (label != null) _choiceLabels.Add(label);
        }
    }

    /// <summary>
    /// Change the displayed text of a choice by index (0-based).
    /// Optionally run it through the translator to match current encoding.
    /// Example: DialogueManagerIntegrated.Instance.SetChoiceText(1, journalText);
    /// </summary>
    public void SetChoiceText(int index, string newText, bool encode = true)
    {
        if (current == null) return;
        if (index < 0 || index >= _choiceLabels.Count) return;

        // update the underlying data so you know what's underneath (optional)
        if (_mutateNodeChoiceTextOnSet &&
            current.choices != null &&
            index < current.choices.Length &&
            current.choices[index] != null)
        {
            current.choices[index].choiceText = newText;
        }

        bool doEncode = encode && CipherDecode.instance != null && CipherDecode.instance.encoding;
        var final = doEncode ? translator.Translate(newText) : newText;

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
    /// Re-translate the currently shown choice labels using the latest cipher state.
    /// Call after the Journal updates mappings.
    /// </summary>
    public void RefreshChoiceTexts()
    {
        if (current == null || current.choices == null) return;

        bool encode = (CipherDecode.instance != null && CipherDecode.instance.encoding);

        for (int i = 0; i < _choiceLabels.Count && i < current.choices.Length; i++)
        {
            var raw = current.choices[i]?.choiceText ?? "";
            var final = encode ? translator.Translate(raw) : raw;
            _choiceLabels[i].text = final;
        }
    }

    void OnChoiceSelected(Choice c)
    {
        if (c.isCorrect)
        {
            DialogueController.Instance.ClearChoices();
            DialogueController.Instance.SetDialogueText("Correct!");

            if (!string.IsNullOrEmpty(c.loadSceneOnSelect))
            {
                StopAllCoroutines();
                StartCoroutine(AutoLoadScene(c.loadSceneOnSelect, 1.0f));
                return;
            }
        }
        else
        {
            DialogueController.Instance.ClearChoices();
            DialogueController.Instance.SetDialogueText("Incorrect.");
        }

        if (c.next != null) ShowNode(c.next);
        else EndConversation();
    }

    IEnumerator AutoLoadScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator AutoNext(DNode next, float delay)
    {
        yield return new WaitForSeconds(Mathf.Max(0f, delay));
        ShowNode(next);
    }

    public void EndConversation()
    {
        DialogueController.Instance.ShowDialogueUI(false);
        active = null;
        current = null;
        StopAllCoroutines();


        isFinished = true;
    }
}

