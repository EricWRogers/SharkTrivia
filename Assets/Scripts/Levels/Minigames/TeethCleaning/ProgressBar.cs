using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Header("Progress Settings")]
    public int swipesAmount = 0;
    public Slider progressBar;

    private int swipeCount = 0;
    private bool isSwiping = false;
    private int lastPercentage = 0;
    
    private Vector2 mousePosition;

    [SerializeField] public Timer timer;

    private Camera mainCamera;

    public SpriteRenderer dirtyTeeth;
    public SpriteRenderer cleanTeeth;
    public SpriteRenderer toothBrush;
    
    public SpriteRenderer drill;

    //connecting to win screen
    public WinScreen winScreen;

    //colliders for teeth
    public Collider2D topTeeth;
    public Collider2D bottomTeeth;



    void Start()
    {
        mainCamera = Camera.main;
        dirtyTeeth.sortingOrder = 1; 
        cleanTeeth.sortingOrder = 1;
        toothBrush.sortingOrder = 3;
        drill.sortingOrder = 3;


        if (progressBar != null)
        {
            progressBar.maxValue = swipesAmount;
            progressBar.value = 0;
        }
        if (dirtyTeeth != null)
        {
            SetAlpha(dirtyTeeth, 1f);
        }
        if (cleanTeeth != null)
        {
            SetAlpha(cleanTeeth, 1f);
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
                //Debug.Log("Started swipe");
            }
        }
        if (Input.GetMouseButton(0) && isSwiping)
        {
            if(!IsMouseOver(mouseWorldPos))
            {
                isSwiping = false;
                return;
            }
            float distance = Vector2.Distance(mousePosition, mouseWorldPos);
            //Debug.Log($"LastPos: {mousePosition} CurrentPos: {mouseWorldPos} Distance: {distance}");
            //Debug.Log("Distance moved " + distance);
            if(distance > 0.4f)
            {
                RegisterSwipe();
                mousePosition = mouseWorldPos;

            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
        }
        UpdateTeethFade();
    }
    private void UpdateTeethFade()
    {
        if (dirtyTeeth != null && progressBar != null && progressBar.maxValue > 0)
        {
            float normalized = progressBar.value / progressBar.maxValue;

            Color c = dirtyTeeth.color;
            c.a = 1f - normalized;
            dirtyTeeth.color = c;
        }

    }
    private void SetAlpha(SpriteRenderer sprite, float alpha)
    {
        Color c = sprite.color;
        c.a = Mathf.Clamp01(alpha);
        sprite.color = c;
    }
    private bool IsMouseOver(Vector2 mouseWorldPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
        if (hit == null)
        {
            return false;
        }
        return hit == topTeeth || hit == bottomTeeth;
    }
    private void RegisterSwipe()
    {
        if (swipeCount >= swipesAmount) return;

        swipeCount++;
        //Debug.Log("Swipe detected " + swipeCount);

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
        //Debug.Log("Swipe goal reached!");
        
        //bonus points for 100% clean
        if(lastPercentage == 100)
        {
            Debug.Log("Perfect clean, bonus awarded");
            ScoreManager.instance.AddPoints(10);
        }
        
        if(TotalScore.instance != null){
            int finalPoints = ScoreManager.instance.GetScore();
            //TotalScore.instance.AddPoints(finalPoints);
            //This gets called when the bar reaches 100% and it detects a swipe
            //TotalScore also gets called in WinScreen.DisplayWinResults so it was doubling the score
        }
        if(Holes.holesRemaining<=0)
        {
            if (timer != null)
                timer.StopTimer();

            //display winscreen when minigame is over
            //winScreen.ShowWinScreen(); this just turns the WinScreen on
            //WinScreen.DisplayWinResults displays the updated scoreboard
            winScreen.DisplayWinResults(ScoreManager.instance.score);
        }
    }

}
