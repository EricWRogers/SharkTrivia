using UnityEngine;

/// One selectable option in a dialogue node.
/// Selecting this will advance the conversation to <see cref="nextNode"/>.

[System.Serializable]   
public class BackstageDialogueChoice
{
    public string choiceText; /// The text shown on the choice button
    public BackstageDialogueNode nextNode; /// The dialogue node to jump to if this choice is clicked.
}
