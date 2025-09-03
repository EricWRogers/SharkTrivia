using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public float force;

    private bool bowling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bowling)
        {
            // Horizontal movement before launch
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 newPosition = transform.position + new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, -5f, 5f); // Adjust lane bounds
            transform.position = newPosition;

            // Launch the ball
            if (Input.GetButtonDown("Fire1")) // Default is Left Mouse Button or Ctrl
            {
                rb.AddForce(transform.forward * force, ForceMode.Impulse);
                bowling = true;
            }
        }

    }
}
