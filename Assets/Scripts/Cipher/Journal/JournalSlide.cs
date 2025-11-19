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
    private GameObject journal;
    public float closeEnoughVal = 0.1f;

    void Start()
    {
        journal = GameObject.Find("JournalUIPrefab");
        originalPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (buttonPressed)
        {
            Vector3 target = isDown ? targetTransform.position : originalPosition;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);

            if(Vector2.Distance(transform.position, target) < closeEnoughVal)
            {
                buttonPressed = false;
                isDown = !isDown;

                if(isDown)
                    journal.transform.GetChild(2).gameObject.SetActive(true);
                else
                    journal.transform.GetChild(1).GetChild(0).GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    public void JournalOpen()
    {
        GetComponent<AudioSource>().pitch = 1f + Random.Range(-0.1f, 0.2f);
        GetComponent<AudioSource>().Play();
        buttonPressed = true;
    }
}
