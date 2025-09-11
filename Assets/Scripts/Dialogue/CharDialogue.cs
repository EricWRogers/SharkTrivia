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

    [Tooltip("Automatically end dialogue. They sync up with the array above.")]
    public bool[] endDialogueLines;

    public DialogueChoice[] choices;

    [Tooltip("Speed at which the text comes out")]
    public float typingSpeed = 0.05f;

    [Tooltip("the beepboops that play when a character speaks. ")]
    public AudioClip voiceSound;
    public float voicePitch = 1f;

    public ScriptableObject[] lineNode;


    public void LoadAllDialogueChildren()
    {
        
        lineNode = Resources.LoadAll<LinesNode>("Resources");
        Debug.Log("Calling all children" + lineNode.Length);
    }
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex; //dialogue index where choices appear
    public string[] choices; //player response options
    public int[] nextDialogueIndexes; //where choice leads
}

