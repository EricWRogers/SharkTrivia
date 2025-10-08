using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonRename : MonoBehaviour
{
    public CipherDecode cipherDecode;
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
        returnText = _text.GetComponent<TMP_Text>();
        titleText.text = "guess letter \"" + _c + "\""; 
    }

    public void Close(string _c)
    {
        returnText.text = _c;
    }

}
