using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerBar : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject UISlider; // parent GameObject containing slider & text
    public Slider powerSlider;
    public Image fillImage;
    public TMP_Text percentageText;

    [Header("Power Settings")]
    public float minPower = 5f;
    public float maxPower = 20f;
    public float chargeSpeed = 20f;

    private float currentPower;
    private bool isChargingUp = true;
    private bool isLocked = false;

    [Header("Ball Reference")]
    public Rigidbody bowlingBall;

    void Start()
    {
        if (powerSlider != null)
        {
            powerSlider.minValue = 0f;
            powerSlider.maxValue = 1f; // Normalized between 0 and 1
            powerSlider.value = 0f;
        }
        currentPower = 0f;

        if (UISlider != null)
            UISlider.SetActive(true); // show UI at start
    }

    void Update()
    {
        if (isLocked) return;

        // Ping-pong charging
        if (isChargingUp)
        {
            currentPower += (chargeSpeed / 100f) * Time.deltaTime;
            if (currentPower >= 1f)
            {
                currentPower = 1f;
                isChargingUp = false;
            }
        }
        else
        {
            currentPower -= (chargeSpeed / 100f) * Time.deltaTime;
            if (currentPower <= 0f)
            {
                currentPower = 0f;
                isChargingUp = true;
            }
        }

        // Update UI
        if (powerSlider != null)
            powerSlider.value = currentPower;

        if (percentageText != null)
            percentageText.text = Mathf.RoundToInt(currentPower * 100f) + "%";

        UpdateColor();
    }

    void UpdateColor()
    {
        if (fillImage == null) return;

        if (currentPower < 0.33f)
            fillImage.color = Color.green;
        else if (currentPower < 0.66f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;
    }

    // Called from BowlingBall when launching
    public void LockPower()
    {
        isLocked = true;

        if (UISlider != null)
            UISlider.SetActive(false); // hide while ball rolls

        // Resets % to 0 to stop moving while ball rolls
        currentPower = 0f;
        if (powerSlider != null)
            powerSlider.value = 0f;
        if (percentageText != null)
            percentageText.text = "0%";
    }

    public void ResetBar()
    {
        currentPower = 0f;
        isLocked = false;
        isChargingUp = true;

        if (powerSlider != null)
            powerSlider.value = 0f;

        if (percentageText != null)
            percentageText.text = "0%";

        if (UISlider != null)
            UISlider.SetActive(true); // show UI again for next throw
    }

    public float GetPowerPercent()
    {
        return currentPower;
    }
}
