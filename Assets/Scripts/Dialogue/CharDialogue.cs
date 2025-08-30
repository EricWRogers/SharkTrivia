using UnityEngine;

[CreateAssetMenu(fileName = "NewCharDialogue", menuName = "Character Dialogue")]
public class CharDialogue : ScriptableObject
{
    [Tooltip("Character name")]
    public string charName;

    [Tooltip("Character portrait image")]
    public Sprite charPortrait;

    [Tooltip("All of their dialogue")]
    public string[] dialogueLines;

    [Tooltip("Whether or not the dialogue automatically continues. They sync up with the array above.")]
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;

    [Tooltip("Speed at which the text comes out")]
    public float typingSpeed = 0.05f;

    [Tooltip("the beepboops that play when a character speaks. ")]
    public AudioClip voiceSound;
    public float voicePitch = 1f;
}
