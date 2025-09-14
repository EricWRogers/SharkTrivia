#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(Conversation))]
public class ConversationEditor : Editor
{
    Conversation convo;

    // temp field        to pick a parent node in the inspector
    DNode parentForChild;

    void OnEnable() => convo = (Conversation)target;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("entry"));

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Nodes", EditorStyles.boldLabel);

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Add Root Node"))
            {
                var node = CreateNode("Root");
                convo.entry = node;
                EditorUtility.SetDirty(convo);
            }
            if (GUILayout.Button("Add Orphan Node"))
            {
                CreateNode("Node");
            }
        }

        EditorGUILayout.Space(8);
        parentForChild = (DNode)EditorGUILayout.ObjectField(
            new GUIContent("Add Child To"),
            parentForChild, typeof(DNode), false);

        if (parentForChild && GUILayout.Button("Add Choice + Child Node"))
        {
            var child = CreateNode(parentForChild.name + "_Child");

            var list = (parentForChild.choices != null)
                ? parentForChild.choices.ToList()
                : new List<Choice>();

            list.Add(new Choice { choiceText = "…", next = child });
            parentForChild.choices = list.ToArray();

            EditorUtility.SetDirty(parentForChild);
            EditorUtility.SetDirty(child);
        }

        EditorGUILayout.Space(8);
        if (GUILayout.Button("Ping All Nodes"))
        {
            foreach (var n in AssetDatabase.LoadAllAssetsAtPath(
                         AssetDatabase.GetAssetPath(convo)).OfType<DNode>())
                EditorGUIUtility.PingObject(n);
        }

        serializedObject.ApplyModifiedProperties();
    }

    DNode CreateNode(string nameHint)
    {
        var node = ScriptableObject.CreateInstance<DNode>();
        node.name = nameHint;

        AssetDatabase.AddObjectToAsset(node, convo); // sub-asset in the same file
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(convo));
        EditorUtility.SetDirty(node);
        return node;
    }
}
#endif
