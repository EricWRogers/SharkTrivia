using UnityEngine;


//this is the new root of the integrated Dialogue system
[CreateAssetMenu(menuName = "BackstageDialogue/Conversation")]
public class Conversation : ScriptableObject
{
    public DNode entry;   // first line of this conversation
}
