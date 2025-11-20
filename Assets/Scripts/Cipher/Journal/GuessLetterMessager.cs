using UnityEngine;
using UnityEngine.UI;

public class GuessLetterMessager : MonoBehaviour
{
    public ButtonRename buttonRename;
    public Translator translator;
    public string letter;
    public GameObject englishAlphabet;
    public GameObject guessLetter;
   // public GameObject unassignLetter;
    public CanvasGroup gLButtons;
    public Button unassButton;
    public char unassCipherLetter;

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
        guessLetter.SetActive(true);
        //Take letter as enlgih key from GLM

        Debug.Log(CipherDecode.instance.charAssignments[letter.ToLower().ToCharArray()[0]]);
        Debug.Log(letter.ToLower().ToCharArray()[0]);

        if (CipherDecode.instance.charAssignments[letter.ToLower().ToCharArray()[0]] != '~')
        {

            if (gLButtons != null)
            {
                gLButtons.interactable = false;
            }
            GameObject.Find("Unassign").GetComponent<Button>().interactable = true;
            Debug.Log("on?");

            //unassCipherLetter = '~';


        }

        else
        {
            GameObject.Find("Unassign").GetComponent<Button>().interactable = false;
            //Debug.Log(unassButton.gameObject.name);
            gLButtons.interactable = true;
        }


    }
}
