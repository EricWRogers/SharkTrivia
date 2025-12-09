using UnityEngine;
using TMPro;

public class ButtonRename : MonoBehaviour
{
    public char keyToUpdate;
    public char newKeyValue;
    public TMP_Text returnText;
    public TMP_Text titleText;

    private bool dumbBool = false;

    // void Start()
    // {
    //     if (CipherDecode.instance.confirmedCharAssignments.ContainsValue(newKeyValue))
    //     {
    //         returnText.text = $"{CipherDecode.instance.confirmedCharAssignments[newKeyValue]}";
    //     }
    // }

    void FixedUpdate()
    {
        if(CipherDecode.instance != null && !dumbBool)
        {
            if (CipherDecode.instance.confirmedCharAssignments.ContainsValue(char.ToLower(newKeyValue))) // this is very jank, ugly hack - Scott
            {
                char i;

                for(i = 'a'; i < 'z'; i++)
                {
                    if(CipherDecode.instance.confirmedCharAssignments[i] == char.ToLower(newKeyValue))
                        break;
                }

                returnText.text = $"{i}";
            } 
            dumbBool = true;
        }
    }

    public void Open(GameObject _text, string _c)
    {
        newKeyValue = _c[0];
        returnText = _text.GetComponent<TMP_Text>();
        titleText.text = "guess letter \"" + _c + "\""; 
    }

    public void Close(string _c)
    {

        if(_c[0] != '~'){

            // if(CipherDecode.instance.charAssignments[_c[0]] == '~' && CipherDecode.instance.confirmedCharAssignments[_c[0]] == '~')
            //     returnText.text = _c;

            if (CipherDecode.instance == null)
                Debug.Log("Cipher is null");

            
            if(CipherDecode.instance.CharAssignment(_c[0], newKeyValue) <= 0) // ugly jank
                returnText.text = _c;


            //THIS IS INCREDIBLY SCUFFED ALSO, YOU KNOW THE DRILL
            if((CipherDecode.instance.isCountingGuesses || CipherDecode.instance.isLimitingCorrectness) && (CipherDecode.instance.numGuesses == CipherDecode.instance.maxGuesses || CipherDecode.instance.numCorrectGuesses == CipherDecode.instance.maxCorrectGuesses))
            {
                var buttonScripts = FindObjectsByType<GuessLetterMessager>(FindObjectsSortMode.None);

                for(int i = 0; i < buttonScripts.Length; i++)
                {
                    buttonScripts[i].OnJournalEnter();
                }
            }
        }
        else
            ClearLetter(_c);
    }

    public void ClearLetter(string _u)
    {
        returnText.text = _u;

        Debug.Log($"Clear letter called, key is {GuessLetterMessager.mostRecentChar}");

        CipherDecode.instance.CharAssignment(GuessLetterMessager.mostRecentChar, '~');//charAssignments[_u[0]] = newKeyValue;
        CipherDecode.instance.unassButtonsToEnable[char.ToLower(newKeyValue) - 'a'] = false;


        //THIS IS INCREDIBLY SCUFFED ALSO, YOU KNOW THE DRILL
        if((CipherDecode.instance.isCountingGuesses || CipherDecode.instance.isLimitingCorrectness) && (CipherDecode.instance.numGuesses == CipherDecode.instance.maxGuesses || CipherDecode.instance.numCorrectGuesses == CipherDecode.instance.maxCorrectGuesses))
        {
            var buttonScripts = FindObjectsByType<GuessLetterMessager>(FindObjectsSortMode.None);

            for(int i = 0; i < buttonScripts.Length; i++)
            {
                buttonScripts[i].OnJournalEnter();
            }
        }
    }

}
