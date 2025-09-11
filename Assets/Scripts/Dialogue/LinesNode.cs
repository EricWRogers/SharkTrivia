using UnityEngine;

[CreateAssetMenu(fileName = "NewLineNode", menuName = "Dialogue Line Node")]
public class LinesNode : ScriptableObject 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Tooltip("Character name")]
    public string charName;

    [Tooltip("Character portrait image")]
    public Sprite charPortrait;

    [Tooltip("The line of dialogue")]
    public string dialogueLines;

    [Tooltip("Whether or not the dialogue automatically continues. They sync up with the array above.")]
    public bool autoProgressLines;
}
