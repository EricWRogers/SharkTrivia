using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Wager : MonoBehaviour
{
    public Slider slider;
    public TMP_Text sliderText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.maxValue = TotalScore.instance.totalScore;
        slider.onValueChanged.AddListener((v) =>
        {
            sliderText.text = "Wager Amount: " + v.ToString();
        });
    }

    // Update is called once per frame
    void Update()
    {
        TotalScore.instance.wager = slider.value;
    }
}
