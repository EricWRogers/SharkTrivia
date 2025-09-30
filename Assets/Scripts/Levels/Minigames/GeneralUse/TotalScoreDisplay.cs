using UnityEngine;
using TMPro;

public class TotalScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;
    
    void Update(){
        if(TotalScore.instance != null){
            scoreText.text = "Total Score: "+ TotalScore.instance.GetScore();
        }
    }

}