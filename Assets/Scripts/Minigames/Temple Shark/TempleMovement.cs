using Unity.VisualScripting;
using UnityEngine;

public class TempleMovement : MonoBehaviour
{
    public float runSpeed = 2f;
    public float movementSpeed = 5f;
    public float rightLimit = 5.5f;
    public float leftLimit = -5.5f;

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal"); // A & D or Left & Right
        transform.Translate(Vector3.forward * Time.deltaTime * runSpeed, Space.World);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (this.gameObject.transform.position.x > leftLimit)
            {
                transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
            }
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (this.gameObject.transform.position.x < rightLimit)
            {
                transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
            }
        }
    }
}
