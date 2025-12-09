using UnityEngine;

public class ToolCollisionToggle : MonoBehaviour
{
    private Collider2D[] colliders;

    void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>(true);
    }

    public void EnableColliders(bool enabled)
    {
        foreach (var col in colliders)
            col.enabled = enabled;
    }
}
