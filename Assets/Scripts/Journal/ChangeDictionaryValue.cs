using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor.Animations;
using JetBrains.Annotations;
using System.Collections;

public class ChangeDictionaryValue : MonoBehaviour
{
    public List<char> keys;
    public List<char> values;

    public Dictionary<char, char> charAssignments = new Dictionary<char, char> //dictionary by Scott
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

    // Update is called once per frame
    public void OnButtonClick()
        {
            Debug.Log("A Button");
            
        }
}
