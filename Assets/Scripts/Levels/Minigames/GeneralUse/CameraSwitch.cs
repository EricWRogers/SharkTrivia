using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Tooltip("Camera to be turned turned OFF.")]
    public GameObject camera1;
    [Tooltip("Camera to be turned turned ON.")]
    public GameObject camera2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            camera1.SetActive(false);
            camera2.SetActive(true);

            // Disable this until reset
            gameObject.SetActive(false);
        }
    }
}
