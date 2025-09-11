using UnityEngine;


/// controls all the dialogue flow: shows nodes in the bubble UI and moves based on choice clicks.
public class BackStageDialogueManager : MonoBehaviour
{
    public static BackStageDialogueManager Instance { get; private set; }
    public BackStageDialogueBubble bubble;
    BackstageDialogueNode current;   /// The node currently being displayed.

    void Awake() => Instance = this;

/// Begin a dialogue session from the graph's entry node.
    public void StartDialogue(BackstageDialogueGraph graph)
    {
        if (!graph || !graph.entry) return;
        ShowNode(graph.entry);
    }

  /// Render a specific node, show text and add choice buttons 
    void ShowNode(BackstageDialogueNode node)
    {
        current = node;
        bubble.Show(node.speakerName, node.speakerLine);
        bubble.ClearChoices();

        if (node.choices != null && node.choices.Length > 0)
        {
            foreach (var c in node.choices)
            {
                var next = c.nextNode; // capture
                bubble.AddChoice(c.choiceText, () => ShowNode(next));
            }
        }
        else
        {
            // if no choices are defined, show a single continue button.
            bubble.AddChoice("Continue", () =>
            {
                if (node.nextIfNoChoices) ShowNode(node.nextIfNoChoices);
                else EndDialogue();
            });
        }
    }

 /// Closes the UI and clears state.
    public void EndDialogue()
    {
        bubble.Hide();
        current = null;
    }
}
