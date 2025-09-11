using UnityEngine;
using System.Collections;

public class DialogueManagerIntegrated : MonoBehaviour
{
    public static DialogueManagerIntegrated Instance { get; private set; }
    void Awake() { Instance = this; }

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
        current = node;

        // UI on
        var ui = DialogueController.Instance;
        ui.ShowDialogueUI(true);
        ui.SetCharInfo(node.speakerName, node.portrait);
        ui.SetDialogueText(node.speakerLine);
        ui.ClearChoices();

        // Branching
        if (node.choices != null && node.choices.Length > 0)
        {
            foreach (var c in node.choices)
            {
                var next = c.next; // capture
                ui.CreateChoiceButton(c.choiceText, () => ShowNode(next));
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
