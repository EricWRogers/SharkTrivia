using UnityEngine;

public class Toothbrush : MonoBehaviour
{
    public bool isActive = false; 
    private Vector2 mousePos;
    public float moveSpeed = 1f;

    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = Vector2.Lerp(transform.position, mousePos, moveSpeed);
    }
}
