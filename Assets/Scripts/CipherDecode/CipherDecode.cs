using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor.Animations;
using JetBrains.Annotations;

// //Originally Programmed by Samuel (Scott)

public class CipherDecode : MonoBehaviour
{

    public char englishChar;
    public char cipherButtonChar;

    public Dictionary<char, char> charAssignments = new Dictionary<char, char>
    {
        //tilde represents an english character which has not been assigned a ciphertext equivalent 
        {'a', '~'},{'b', '~'},{'c', '~'},{'d', '~'},
        {'e', '~'},{'f', '~'},{'g', '~'},{'h', '~'},
        {'i', '~'},{'j', '~'},{'k', '~'},{'l', '~'},
        {'m', '~'},{'n', '~'},{'o', '~'},{'p', '~'},
        {'q', '~'},{'r', '~'},{'s', '~'},{'t', '~'},
        {'u', '~'},{'v', '~'},{'w', '~'},{'x', '~'},
        {'y', '~'},{'z', '~'}
    };

    public void CharAssignment()
    {
        //Debug.Log(cipherButtonID, cipherButtonGroup);

        int englishCharIndex = 0;
        //englishCharIndex = WhoAmI(cipherButtonGroup.transform.parent.gameObject);
        //0-25 corresponds to letters

        englishChar = 'a';
        cipherButtonChar = '~';

        //this if else block handles assigning the characters in the dictionary or resetting them to '~' if the button is hit again
        if(charAssignments[cipherButtonChar] != englishChar)
        {
            charAssignments[cipherButtonChar] = englishChar;
        }
        else {
            charAssignments[cipherButtonChar] = '~';
        }

        Debug.Log(cipherButtonChar);

    }
}
