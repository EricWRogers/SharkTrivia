using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float power;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.forward * power);
        }
    }
}
