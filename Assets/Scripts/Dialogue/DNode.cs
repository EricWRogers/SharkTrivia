using UnityEngine;

[System.Serializable]                       // a single button in the UI
public class Choice
{
    public string choiceText = "…";
    public DNode next;                      // child node stored as sub-asset
}

[CreateAssetMenu(menuName = "BackstageDialogue/Node")] 
public class DNode : ScriptableObject
{
    [Header("Speaker")]
    public string speakerName = "???";
    public Sprite portrait;

    [Header("Line")]
    [TextArea(2, 6)] public string speakerLine;

    [Header("Flow")]
    public bool autoProgress = false;       // if true and no choices, auto-continue
    public float autoDelay = 0.75f;         // delay before auto-continue
    public DNode nextIfNoChoices;           // fallback

    [Header("Branching")]
    public Choice[] choices;                // children 
}
