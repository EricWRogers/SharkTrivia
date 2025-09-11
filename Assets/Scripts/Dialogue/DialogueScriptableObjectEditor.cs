using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharDialogue))]
public class DialogueScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharDialogue charDialogue = (CharDialogue)target;

        if(GUILayout.Button("Add Dialogue Instance"))
        {
            CreateNestedAsset(charDialogue);
        }
        if(GUILayout.Button("Collect All Children"))
        {
            charDialogue.LoadAllDialogueChildren();
        }
    }

    private void CreateNestedAsset(CharDialogue _charDialogue)
    {
        //create an instance of the child (dialogue line)
        LinesNode linesNode = ScriptableObject.CreateInstance<LinesNode>();

        linesNode.name = "NewDialogueLineNode";

        //add child as subasset of parent
        if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(_charDialogue)))
        {
            AssetDatabase.AddObjectToAsset(linesNode, _charDialogue);
            AssetDatabase.SaveAssets();
        }
        else
        {
            Debug.Log("Parent asset must be saved to a file to add sub assets");
        }

        //update project window to show new child
        EditorUtility.SetDirty(linesNode);

    }
}
