using UnityEngine;
using TMPro;
using System.Collections.Generic;

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
        returnText.text = _c;

        if (CipherDecode.instance == null)
            Debug.Log("Cipher is null");
        CipherDecode.instance.CharAssignment(newKeyValue, _c[0]);


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
