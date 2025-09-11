using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Header("Progress Settings")]
    public int swipesAmount = 15;
    public Slider progressBar;

    private int swipeCount = 0;
    private bool isSwiping = false;
    private int lastPercentage = 0;
    
    private Vector2 mousePosition;

    [SerializeField] private Timer timer;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (progressBar != null)
        {
            progressBar.maxValue = swipesAmount;
            progressBar.value = 0;
        }
    }
    void Update()
    {
        
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("Mouse world position " + mouseWorldPos);
        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOver(mouseWorldPos))
            {
                isSwiping = true;
                mousePosition = mouseWorldPos;
                Debug.Log("Started swipe");
            }
        }
        if (Input.GetMouseButton(0) && isSwiping)
        {
            float distance = Vector2.Distance(mousePosition, mouseWorldPos);
            Debug.Log($"LastPos: {mousePosition} CurrentPos: {mouseWorldPos} Distance: {distance}");
            Debug.Log("Distance moved " + distance);
            if(distance > 5f)
            {
                RegisterSwipe();
                mousePosition = mouseWorldPos;

            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
        }
    }
    private bool IsMouseOver(Vector2 mouseWorldPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        return hit.collider != null && hit.collider == GetComponent<Collider2D>();
    }
    private void RegisterSwipe()
    {
        if (swipeCount >= swipesAmount) return;

        swipeCount++;
        Debug.Log("Swipe detected " + swipeCount);

        if(progressBar != null)
        {
            progressBar.value = swipeCount;
        }
        //calculates percentage progress
        float percentage = ((float)swipeCount / swipesAmount) * 100;
        int roundedPercentage = Mathf.RoundToInt(percentage);

        if(roundedPercentage > lastPercentage)
        {
            int pointsToAward = roundedPercentage - lastPercentage;
            ScoreManager.instance.AddPoints(pointsToAward);
            lastPercentage = roundedPercentage;
        }
        if(swipeCount>= swipesAmount)
        {
            OnSwipeGoalReached();
        }
    }

    private void OnSwipeGoalReached()
    {
        Debug.Log("Swipe goal reached!");
        if (timer != null)
        {
            timer.StopTimer();
        }
        
        //bonus points for 100% clean
        if(lastPercentage == 100)
        {
            Debug.Log("Perfect clean, bonus awarded");
            ScoreManager.instance.AddPoints(10);
        }
    }

}
