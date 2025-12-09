using UnityEngine;

public class ToolCollisionToggle : MonoBehaviour
{
    [Header("Which tools can affect this object?")]
    public bool worksWithToothbrush;
    public bool worksWithPick;
    public bool worksWithDrill;

    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        UpdateColliderState();
    }

    public void UpdateColliderState()
    {
        if (ToolManager.ActiveToolName == "Toothbrush")
            col.enabled = worksWithToothbrush;

        else if (ToolManager.ActiveToolName == "Pick")
            col.enabled = worksWithPick;

        else if (ToolManager.ActiveToolName == "Drill")
            col.enabled = worksWithDrill;
    }
}
