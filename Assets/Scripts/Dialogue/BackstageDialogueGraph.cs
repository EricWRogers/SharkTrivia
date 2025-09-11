using UnityEngine;


/// A ScriptableObject that represents a whole dialogue graph like a conversation
/// You assign an entry node that acts as the starting point when the dialogue starts

[CreateAssetMenu(menuName = "BackstageDialogue/Graph")]
public class BackstageDialogueGraph : ScriptableObject
{
    public BackstageDialogueNode entry;
}
