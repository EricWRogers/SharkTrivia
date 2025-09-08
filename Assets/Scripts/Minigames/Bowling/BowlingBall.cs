using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float maxPower = 800f; // max force
    private bool hasLaunched = false;
    private PowerBar powerBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        powerBar = FindAnyObjectByType<PowerBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasLaunched)
        {
            // Get the power % from the power bar
            float percent = (powerBar != null) ? powerBar.GetPowerPercent() : 1f;

            // Calculate final force
            float launchPower = maxPower * percent;

            // Add forward force
            rb.AddForce(Vector3.forward * launchPower, ForceMode.Impulse);

            hasLaunched = true;
        }
    }

    public void ResetBall()
    {
        hasLaunched = false; // allows launch again after reset
    }
}
