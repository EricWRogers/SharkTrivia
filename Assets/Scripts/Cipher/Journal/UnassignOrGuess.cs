using UnityEngine;

public class UnassignOrGuess : MonoBehaviour
{
    public CipherDecode cipherDecode;
    public char englishKey;
    public GameObject englishAlphabet;
    public GameObject guessLetter;
    public GameObject unassignLetter;

    public void OnClick()
    {
        englishAlphabet.SetActive(false);

        if (cipherDecode.charAssignments[englishKey] != '~')
        {
            unassignLetter.SetActive(true);
        }

        else
        {
            guessLetter.SetActive(true);
        }


    }
    
}
