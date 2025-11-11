using UnityEngine;

public class Drill : MonoBehaviour
{
    private Vector2 mousePos;
    public float moveSpeed = 0.2f; 

    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = Vector2.Lerp(transform.position, mousePos, moveSpeed);
    }
}
