using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonValueRandomizer : MonoBehaviour
{
    public CipherDecode cipherDecode;
    public char buttonTextB;
    public char keyToUpdate = 'a';
    public char newKeyValue;

    public void NewText()
    {
        buttonTextB = 'B';
        newKeyValue = buttonTextB;
    }

    // public void DictUpdate()
    // {
    //     if (cipherDecode.charAssignments.ContainsKey(keyToUpdate))
    //     {
    //         cipherDecode.charAssignments[keyToUpdate] = newKeyValue; 

    //     }
    // }

}
