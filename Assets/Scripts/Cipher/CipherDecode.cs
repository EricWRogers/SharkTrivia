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

    public List<char> keys = new List<char>();
    public List<char> values = new List<char>();

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
        for (int i = 97; i < 122; i++)
        {
            if (charAssignments.ContainsKey((char)i))
            {
                
            }
        }

    }
}
