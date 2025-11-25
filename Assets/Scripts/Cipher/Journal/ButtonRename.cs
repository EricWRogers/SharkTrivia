using UnityEngine;
using TMPro;

public class ButtonRename : MonoBehaviour
{
    public char buttonText;
    public char keyToUpdate;
    public char newKeyValue;
    public TMP_Text returnText;
    public TMP_Text titleText;


    public void NewText()
    {
        newKeyValue = buttonText;
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

            if(CipherDecode.instance.charAssignments[_c[0]] == '~' && CipherDecode.instance.confirmedCharAssignments[_c[0]] == '~')
                returnText.text = _c;

            if (CipherDecode.instance == null)
                Debug.Log("Cipher is null");

            
            CipherDecode.instance.CharAssignment(_c[0], newKeyValue);


            //THIS IS INCREDIBLY SCUFFED ALSO, YOU KNOW THE DRILL
            if((CipherDecode.instance.isCountingGuesses || CipherDecode.instance.isLimitingCorrectness) && (CipherDecode.instance.numGuesses == CipherDecode.instance.maxGuesses || CipherDecode.instance.numCorrectGuesses == CipherDecode.instance.maxCorrectGuesses))
            {
                var buttonScripts = FindObjectsByType<GuessLetterMessager>(FindObjectsSortMode.None);

                for(int i = 0; i < buttonScripts.Length; i++)
                {
                    buttonScripts[i].OnEnable();
                }
            }
        }
        else
            ClearLetter(_c);
    }

    public void ClearLetter(string _u)
    {
        returnText.text = _u;

        CipherDecode.instance.CharAssignment(_u[0], newKeyValue);


        //THIS IS INCREDIBLY SCUFFED ALSO, YOU KNOW THE DRILL
        if((CipherDecode.instance.isCountingGuesses || CipherDecode.instance.isLimitingCorrectness) && (CipherDecode.instance.numGuesses == CipherDecode.instance.maxGuesses || CipherDecode.instance.numCorrectGuesses == CipherDecode.instance.maxCorrectGuesses))
        {
            var buttonScripts = FindObjectsByType<GuessLetterMessager>(FindObjectsSortMode.None);

            for(int i = 0; i < buttonScripts.Length; i++)
            {
                buttonScripts[i].OnEnable();
            }
        }
    }

}
