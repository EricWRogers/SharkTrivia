#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;


//written by Aidan M.
[CustomEditor(typeof(DNode))]
public class DNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(10);
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            var red = new GUIStyle(GUI.skin.button);
            red.normal.textColor = Color.red;
            red.fontStyle = FontStyle.Bold;

            if (GUILayout.Button("Delete This Node", red, GUILayout.Width(180)))
            {
                DeleteThisNode((DNode)target);
            }
        }
    }

    void DeleteThisNode(DNode node)
    {
        // Confirm
        if (!EditorUtility.DisplayDialog(
                "Delete Node",
                $"Delete node '{node.name}'?\n\n" +
                "All links to this node will be cleared.",
                "Delete", "Cancel"))
            return;

        // Find owning Conversation and all nodes in this asset file
        string path = AssetDatabase.GetAssetPath(node);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Node is not saved as a sub-asset yet.");
            return;
        }

        var all = AssetDatabase.LoadAllAssetsAtPath(path);
        var convo = all.OfType<Conversation>().FirstOrDefault();
        var nodes = all.OfType<DNode>().ToArray();

        if (convo == null)
        {
            Debug.LogError("Could not find Conversation asset owning this node.");
            return;
        }

        // Clear references to this node from every other node
        foreach (var n in nodes)
        {
            // Clear the linear link
            if (n.nextIfNoChoices == node)
            {
                Undo.RecordObject(n, "Clear nextIfNoChoices");
                n.nextIfNoChoices = null;
                EditorUtility.SetDirty(n);
            }

            // Remove choices that point to this node
            if (n.choices != null && n.choices.Length > 0)
            {
                var newChoices = n.choices.Where(c => c != null && c.next != node).ToArray();
                if (newChoices.Length != n.choices.Length)
                {
                    Undo.RecordObject(n, "Remove choice to deleted node");
                    n.choices = newChoices;
                    EditorUtility.SetDirty(n);
                }
            }
        }

        // If this node was the entry, clear it (you can set a new entry later)
        if (convo.entry == node)
        {
            Undo.RecordObject(convo, "Clear conversation entry");
            convo.entry = null;
            EditorUtility.SetDirty(convo);
            Debug.LogWarning($"Deleted entry node; '{convo.name}' now has no Entry.");
        }

        // remove the sub-asset and save
        Undo.RegisterCompleteObjectUndo(convo, "Delete node asset");
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Ping the conversation so the selection doesn't point to a deleted object or anythin
        EditorGUIUtility.PingObject(convo);
    }
}
#endif

