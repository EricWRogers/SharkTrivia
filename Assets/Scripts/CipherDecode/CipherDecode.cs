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

    public List<char> keys;
    public List<char> values;

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

    //KEYS AND VALUES MUST BE THE SAME LENGTH!!!
    public void CharAssignment()
    {

        for (int i = 0; i < keys.Count; i++)
        {
            if (charAssignments.ContainsKey(keys[i]))
            {
                charAssignments[keys[i]] = values[i];
            }
        }

    }
}
