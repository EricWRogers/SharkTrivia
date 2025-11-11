using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public GameObject toothbrush;
    public GameObject drill;

    private GameObject currentTool;

    void Start()
    {
        
        SelectTool(toothbrush);
    }

    void Update()
    {
        // switch tools using 1 or 2
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectTool(toothbrush);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectTool(drill);
    }

    void SelectTool(GameObject tool)
    {
        
        toothbrush.SetActive(false);
        drill.SetActive(false);

        
        tool.SetActive(true);
        currentTool = tool;
    }
}
