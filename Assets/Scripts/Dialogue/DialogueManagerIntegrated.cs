using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DialogueManagerIntegrated : MonoBehaviour
{
    public static DialogueManagerIntegrated Instance { get; private set; }
    public static Translator translator;

    void Awake()
    {
        Instance = this;
        gameObject.AddComponent<Translator>();
        translator = gameObject.GetComponent<Translator>();
    }

    Conversation active;
    DNode current;

    public void StartConversation(Conversation convo)
    {
        if (!convo || !convo.entry) return;
        active = convo;
        ShowNode(convo.entry);
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

        // encode line with your translator
        ui.SetDialogueText(translator.Translate(
            node.speakerLine,
            new List<char> { 'w', 'h', 'o', 'a', 'y', 'e' }
        ));
        ui.ClearChoices();

        // Branching – create choices once
        if (node.choices != null && node.choices.Length > 0)
        {
            foreach (var c in node.choices)
            {
                var choiceCopy = c; 
                ui.CreateChoiceButton(
                    translator.Translate(choiceCopy.choiceText, new List<char> { 'w', 'h', 'o', 'a', 'y', 'e' }),
                    () => OnChoiceSelected(choiceCopy)
                );
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
        ui.CreateChoiceButton("Continue", () =>
        {
            if (node.nextIfNoChoices) ShowNode(node.nextIfNoChoices);
            else EndConversation();
        });
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
    }
}
