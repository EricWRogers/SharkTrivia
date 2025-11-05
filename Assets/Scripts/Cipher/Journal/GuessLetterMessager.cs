using UnityEngine;
using UnityEngine.UI;

public class GuessLetterMessager : MonoBehaviour
{
    public ButtonRename buttonRename;
    public Translator translator;
    public string letter;
    public GameObject englishAlphabet;
    public GameObject guessLetter;
    public GameObject unassignLetter;

    public void Open()
    {
        buttonRename.Open(gameObject.transform.GetChild(0).gameObject, letter);
    }

    public void OnEnable()
    {
        bool maxGuessesIncurred = CipherDecode.instance.numGuesses == CipherDecode.instance.maxGuesses || CipherDecode.instance.numCorrectGuesses == CipherDecode.instance.maxCorrectGuesses;

        if (CipherDecode.instance.confirmedCharAssignments.ContainsValue(letter.ToLower()[0]) || maxGuessesIncurred)
        {
            gameObject.GetComponent<Button>().interactable = false;

            //THIS LINE BELOW IS EXTREMELY SCUFFED ON SEVERAL LEVELS ; MAKE BETTER LATER - Scott
            if (transform.parent.parent.parent.GetChild(transform.parent.parent.parent.childCount - 1).name == "Debug_1" && maxGuessesIncurred)
            {
                transform.parent.parent.parent.GetChild(transform.parent.parent.parent.childCount - 1).gameObject.SetActive(true);
            }
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
        }

        //THIS LINE BELOW IS ALSO EXTREMELY SCUFFED ; ALSO MAKE BETTER LATER - Scott
        if (transform.parent.parent.parent.GetChild(transform.parent.parent.parent.childCount - 1).name == "Debug_1" && !maxGuessesIncurred)
            transform.parent.parent.parent.GetChild(transform.parent.parent.parent.childCount - 1).gameObject.SetActive(false);

    } 
    public void MiniMenuManager()
    {
        englishAlphabet.SetActive(false);

        //Take letter as enlgih key from GLM

        if (CipherDecode.instance.charAssignments[letter.ToLower().ToCharArray()[0]] != '~')
        {
            unassignLetter.SetActive(true);
        }

        else
        {
            guessLetter.SetActive(true);
        }


    }
}
