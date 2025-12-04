using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScroller : MonoBehaviour
{
    public float scrollSensitivity = 1f;
    [Header("UI Elements")]
    public GameObject UISlider; // parent GameObject containing slider & text
    public Slider superSlider;
    public Image fillImage;
    public TMP_Text percentageText;

    [Header("Power Settings")]
    public float minPower = 0f;
    public float maxPower = 10f;

    [Header("Ball Reference")]
    public Rigidbody bowlingBall;

    void Update()
    {
        
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0 && percentageText != null)
        {
            superSlider.value += scrollInput * scrollSensitivity;
            superSlider.value = Mathf.Clamp(superSlider.value, superSlider.minValue, superSlider.maxValue);
            percentageText.text = Mathf.RoundToInt(superSlider.value * 100f) + "%";
        }
        UpdateColor();
    }

    void UpdateColor()
    {
        if (fillImage == null) return;

        if (superSlider.value < 0.33f)
            fillImage.color = Color.green;
        else if (superSlider.value < 0.66f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;
    }
    public float GetPowerPercent()
    {
        return superSlider.value;
    }
}
