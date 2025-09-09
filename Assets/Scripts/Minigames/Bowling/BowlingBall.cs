using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float maxPower = 800f; // max force
    public bool hasLaunched = false;
    private PowerBar powerBar;

    public float spinForce = 200f; // Torque amount (positive = right) (negative = left)
    public float curveStrength = 1f; // How much spin curves the ball
    public float randomSpinRange = 50f; // small random spin added each throw

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

            // Add spin based on player input (A = left, D = right)
            float horizontalSpin = Input.GetAxisRaw("Horizontal"); // -1 (A) / 0 / +1 (D)

            // Random spin (small, unpredictable drift)
            float randomSpin = Random.Range(-randomSpinRange, randomSpinRange);

            // Total spin applied
            float totalSpin = (spinForce * horizontalSpin) + randomSpin;

            if (Mathf.Abs(totalSpin) > 0.01f)
            {
                rb.AddTorque(Vector3.up * totalSpin, ForceMode.Impulse);
            }

            hasLaunched = true;

            // Lock/hide power bar
            if (powerBar != null)
                powerBar.LockPower();
        }
    }

    void FixedUpdate()
    {
        if (hasLaunched)
        {
            // Apply side force proportional to spin
            float spinAmount = rb.angularVelocity.y;
            Vector3 sideForce = Vector3.right * spinAmount * curveStrength;
            rb.AddForce(sideForce);
        }
    }

    public void ResetBall()
    {
        hasLaunched = false; // allows launch again after reset
    }
}
