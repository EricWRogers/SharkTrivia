using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public GameObject toothbrush;
    public GameObject drill;
    public GameObject pick;

    public static string ActiveToolName = "Toothbrush"; 
    [HideInInspector] public GameObject currentTool;

    void Start()
    {
        SelectTool(toothbrush, "Toothbrush");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectTool(toothbrush, "Toothbrush");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectTool(drill, "Drill");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectTool(pick, "Pick");
    }

    void SelectTool(GameObject tool, string toolName)
    {
        
        toothbrush.SetActive(false);
        drill.SetActive(false);
        pick.SetActive(false);

        
        tool.SetActive(true);
        currentTool = tool;
        ActiveToolName = toolName;

        
        Toothbrush tb = toothbrush.GetComponent<Toothbrush>();
        if (tb != null)
            tb.isActive = (tool == toothbrush);
    }
}
