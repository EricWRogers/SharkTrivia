using UnityEngine;


[CreateAssetMenu(menuName="BackstageDialogue/Node")]
public class BackstageDialogueNode : ScriptableObject
{
    [TextArea(2, 6)] public string speakerLine;
    public string speakerName = "???";
    public BackstageDialogueChoice[] choices; // leave empty to continue
    public BackstageDialogueNode nextIfNoChoices; // linear
}
