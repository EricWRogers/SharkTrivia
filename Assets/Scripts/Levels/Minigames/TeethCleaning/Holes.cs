using UnityEngine;

public class Holes : MonoBehaviour
{
    public static int holesRemaining = 0; 
    public float fadeSpeed = 1.5f;
    private SpriteRenderer sr;
    public SpriteRenderer holes;

    void Start()
    {
        holes.sortingOrder = 2;
        sr = GetComponent<SpriteRenderer>();

        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
            Debug.LogWarning($"{name} has no Collider2D!");
        else
            col.isTrigger = true;

        holesRemaining++; 
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Drill") && Input.GetMouseButton(0))
        {
            Color c = sr.color;
            c.a -= fadeSpeed * Time.deltaTime;
            sr.color = c;

            if (c.a <= 0)
            {   
                holesRemaining--; 
                Destroy(gameObject);
                ScoreManager.instance.AddPoints(5);

                if (holesRemaining <= 0)
                {
                    Debug.Log("All holes cleared!");
                    ProgressBar pb = FindFirstObjectByType<ProgressBar>();
                    if (pb != null && pb.progressBar.value >= pb.progressBar.maxValue)
                    {
                        if (pb.timer != null)
                        pb.timer.StopTimer();

                        pb.winScreen.ShowWinScreen();
                    }
                }
            }
        }
    }
}

