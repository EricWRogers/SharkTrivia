using UnityEngine;

public class Toothbrush : MonoBehaviour
{
    private Vector2 mousePos;
    public float moveSpeed = 1f;    // useing one as a defalt, actuall speed set to 0.5
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
                //****** movement ******
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);    //very important
        transform.position = Vector2.Lerp(transform.position, mousePos, moveSpeed);
    }
}
