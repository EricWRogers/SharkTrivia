#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(Conversation))]
public class ConversationEditor : Editor
{
    Conversation convo;
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
                var node = CreateNodeWithNumber("Rn");   //auto numbered
                convo.entry = node;
                EditorUtility.SetDirty(convo);
            }
            if (GUILayout.Button("Add Orphan Node"))
            {
                CreateNodeWithNumber("N");                  //auto numbered
            }
        }

        EditorGUILayout.Space(8);
        parentForChild = (DNode)EditorGUILayout.ObjectField(
            new GUIContent("Add Child To"),
            parentForChild, typeof(DNode), false);

        if (parentForChild && GUILayout.Button("Add Choice + Child Node"))
        {
            // children are based on the parent name and numbered
            var child = CreateNodeWithNumber(parentForChild.name + "_Child");

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
            foreach (var n in GetAllNodes()) EditorGUIUtility.PingObject(n);
        }

        serializedObject.ApplyModifiedProperties();
    }

    // ---------- helpers ----------

    IEnumerable<DNode> GetAllNodes()
    {
        var path = AssetDatabase.GetAssetPath(convo);
        return AssetDatabase.LoadAllAssetsAtPath(path).OfType<DNode>();
    }

    string GetUniqueName(string prefix)
    {
        var existing = new HashSet<string>(GetAllNodes().Select(n => n.name));
        int i = 1;
        string candidate = prefix + i;
        while (existing.Contains(candidate)) { i++; candidate = prefix + i; }
        return candidate;
    }

    DNode CreateNodeWithNumber(string prefix)
    {
        var node = ScriptableObject.CreateInstance<DNode>();
        node.name = GetUniqueName(prefix);              // key line

        AssetDatabase.AddObjectToAsset(node, convo);    // save as sub-asset
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(node);
        Selection.activeObject = node;                  
        return node;
    }
}
#endif
