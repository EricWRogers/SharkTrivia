using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Slider

public class SliderScroller : MonoBehaviour
{
    public Slider superSlider; // Assign your UI Slider here in the Inspector
    public float scrollSensitivity = 1f; // Adjust this value to control scroll speed

    void Update()
    {
        // Get the mouse scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // If there's scroll input, update the slider value
        if (scrollInput != 0)
        {
            // Adjust the slider's value based on scroll input and sensitivity
            superSlider.value += scrollInput * scrollSensitivity;

            // Optional: Clamp the slider value within its min/max range if needed
            superSlider.value = Mathf.Clamp(superSlider.value, superSlider.minValue, superSlider.maxValue);
        }
    }
}
