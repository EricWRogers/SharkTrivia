using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreAndLives : MonoBehaviour
{
    
    public ScoreManager scoreManager;   //Universal score manager

    public static ScoreAndLives Instance { get; private set; }
    public WinScreen winScreen;  //reference to win screen script to call game over

    [Header("Gameplay")]
    public int startingLives = 3;
    public float invincibleDuration = 1.2f;

    [Header("UI")]
    public TMP_Text scoreText;
    public Image[] heartImages;      // 3 heart images in order
    public Sprite fullHeart, emptyHeart;

    public int Score { get; private set; }
    public int Lives { get; private set; }

    bool invincible;
    Vector3 respawnPoint;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Lives = startingLives;
        UpdateHearts();
        UpdateScore();
        // default respawn
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player) respawnPoint = player.transform.position;
    }

    public void SetRespawn(Vector3 pos) => respawnPoint = pos;

    public void AddScore(int amount)
    {
        scoreManager.AddPoints(2);
        Score += amount;
        UpdateScore();
    }

    public void TakeHit(int damage = 1)
    {
        if (invincible) return;
        Lives = Mathf.Max(0, Lives - damage);
        UpdateHearts();
        if (Lives <= 0)
        {
            //total score
            if(TotalScore.instance != null)
            {
                int finalPoints = ScoreManager.instance != null ? ScoreManager.instance.score : 0;
                TotalScore.instance.AddPoints(finalPoints);
            }
            //call gameover later
            winScreen.DisplayWinResults();
            return;
        }
        // respawn pop
        StartCoroutine(IFrames());
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            // reset position
            player.transform.position = respawnPoint;

            // reset rotation so player faces forward again
            player.transform.rotation = Quaternion.identity;

            // also reset TempleMovement direction if needed
            TempleMovement move = player.GetComponent<TempleMovement>();
            if (move != null) move.ResetTurn();
        }

            System.Collections.IEnumerator IFrames()
            {
                invincible = true;
                yield return new WaitForSeconds(invincibleDuration);
                invincible = false;
            }
        }
    
        void UpdateScore()
        {
            //scoreManager.UpdateScoreText();
            if (scoreText) scoreText.text = Score.ToString();
        }
    
        void UpdateHearts()
        {
            if (heartImages == null) return;
            for (int i = 0; i < heartImages.Length; i++)
            {
                if (!heartImages[i]) continue;
                heartImages[i].sprite = (i < Lives) ? fullHeart : emptyHeart;
            }
        }
    }

