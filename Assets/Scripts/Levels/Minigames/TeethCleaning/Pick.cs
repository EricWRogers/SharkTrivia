using UnityEngine;

public class Pick : MonoBehaviour
{
    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }
}
