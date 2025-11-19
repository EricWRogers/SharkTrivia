using UnityEngine;

public class JournalSlide : MonoBehaviour
{
    public Transform targetTransform;
    private Vector3 originalPosition;
    private Vector3 velocity = Vector3.zero;
    private bool buttonPressed = false;
    public float smoothTime = 0.3f;
    public AudioSource journalPaper;     // Assign in Inspector
    private bool isDown = true;

    void FixedUpdate()
    {

        if (buttonPressed)
        {
            Vector3 target = isDown ? targetTransform.position : originalPosition;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);

            if(Vector2.Distance(transform.position, target) < 0.01){
                buttonPressed = false;

            }
        }
    }

    void JournalOpenClose()
    {
        //make button not interactable
        buttonPressed = true;
    }
}
