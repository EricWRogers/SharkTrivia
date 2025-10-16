using UnityEngine;
using UnityEngine.Events;  

[System.Serializable]                       
public class Choice
{
    public string choiceText = "…";
    public DNode next;
    public bool isCorrect = false;
    public bool isTriviaQuestion = false;
    public string loadSceneOnSelect;

    //per-choice event 
    public UnityEvent onSelected;
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
    public bool autoProgress = false;
    public float autoDelay = 0.75f;
    public DNode nextIfNoChoices;

    [Header("Branching")]
    public Choice[] choices;

    [Header("Events")]
    public UnityEvent onEnter;      // drag functions here to run when this node shows
    
}
