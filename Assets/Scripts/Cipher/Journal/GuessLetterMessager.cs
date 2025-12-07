using UnityEngine;
using UnityEngine.UI;

public class GuessLetterMessager : MonoBehaviour
{
    public ButtonRename buttonRename;
    public string letter;
    public GameObject englishAlphabet;
    public GameObject guessLetter;

    public static char mostRecentChar;


    void Awake()
    {
        OnJournalEnter(); // this is very jank, ugly hack - Scott
    }

    public void Open()
    {
        mostRecentChar = letter.ToLower().ToCharArray()[0];
        buttonRename.Open(gameObject.transform.GetChild(0).gameObject, letter);
    }

    public void OnJournalEnter()
    {
        //Debug.Log("Journal Enter called");
        
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

        // Debug.Log(CipherDecode.instance.charAssignments[letter.ToLower().ToCharArray()[0]]);
        // Debug.Log(letter.ToLower().ToCharArray()[0]);

        int letterIndex = letter.ToLower().ToCharArray()[0] - 'a';

        if (CipherDecode.instance.unassButtonsToEnable[letterIndex])
        //CipherDecode.instance.charAssignments.ContainsKey(CipherDecode.instance.charAssignments[CipherDecode.instance.charAssignments[letter.ToLower().ToCharArray()[0]]]) != '~'
        {

            // if (gLButtons != null)
            // {
            //     gLButtons.interactable = false;
            // }
            GameObject.Find("Unassign").GetComponent<Button>().interactable = true;
            Debug.Log("on?");

            //unassCipherLetter = '~';


        }

        else
        {
            GameObject.Find("Unassign").GetComponent<Button>().interactable = false;
            //Debug.Log(unassButton.gameObject.name);
            //gLButtons.interactable = true;
        }


    }
}
