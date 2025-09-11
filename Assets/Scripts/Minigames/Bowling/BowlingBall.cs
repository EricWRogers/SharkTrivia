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
    public float rampUpTime = 2f; // Time before full hook kicks in

    private float spinDirection = 0f; // -1 left, 0 straight, +1 right
    private float launchTime; // When the ball was launched

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
            LaunchBall();
        }
    }

    private void LaunchBall()
    {
        {
            // Get the power % from the power bar
            float percent = (powerBar != null) ? powerBar.GetPowerPercent() : 1f;

            // Calculate final force
            float launchPower = maxPower * percent;

            // Add forward force
            rb.AddForce(Vector3.forward * launchPower, ForceMode.Impulse);

            // Decide spin direction at launch (A = left, D = right)
            spinDirection = Input.GetAxisRaw("Horizontal"); // -1 (A) / 0 / +1 (D)

            // Random spin
            float randomSpin = Random.Range(-randomSpinRange, randomSpinRange);
            float totalSpin = (spinForce * spinDirection) + randomSpin;

            if (Mathf.Abs(totalSpin) > 0.01f)
            {
                rb.AddTorque(Vector3.up * totalSpin, ForceMode.Impulse);
            }

            // Mark launch
            hasLaunched = true;
            launchTime = Time.time;

            // Lock/hide power bar
            if (powerBar != null)
                powerBar.LockPower();
        }
    }

    void FixedUpdate()
    {
        if (hasLaunched)
        {
            float timeSinceLaunch = Time.time - launchTime;

            // Gradually ramp up curve factor
            float curveFactor = Mathf.Clamp01(timeSinceLaunch / rampUpTime);

            // Apply side force proportional to spin
            float spinAmount = rb.angularVelocity.y;
            Vector3 sideForce = Vector3.right * spinAmount * curveStrength * curveFactor;
            rb.AddForce(sideForce);
        }
    }

    public void ResetBall()
    {
        hasLaunched = false; // allows launch again
        spinDirection = 0f;
    }
}
