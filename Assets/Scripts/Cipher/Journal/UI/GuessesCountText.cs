using UnityEngine;
using TMPro;
public class GuessesCountText : MonoBehaviour
{
    public TextMeshProUGUI guessesLeft;

    public GameObject userDecode;
    CipherDecode _cipherDecode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cipherDecode = userDecode.GetComponent<CipherDecode>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cipherDecode.isLimitingCorrectness)
        {
            guessesLeft.text = ("Guesses Left: " + (_cipherDecode.maxCorrectGuesses - _cipherDecode.numCorrectGuesses).ToString());
        }
        else if (_cipherDecode.isCountingGuesses)
        {
            guessesLeft.text = ("Guesses Left: " + (_cipherDecode.maxGuesses - _cipherDecode.numGuesses).ToString());
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
