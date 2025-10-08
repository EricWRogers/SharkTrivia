using UnityEngine;
using TMPro;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;

[CustomEditor(typeof(Translator))]
public class TranslatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Translator tr = (Translator)target;

        if (GUILayout.Button("Translate"))
        {
            tr.usrValues = tr.cipherDecode.GetUsrValues();
            tr.valuesAssigned = tr.cipherDecode.GetValuesAssigned();
            tr.text.text = tr.Translate("the quick brown fox jumps over the lazy dog", tr.keys);
        }
    }
}

public class Translator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    string code = "<style=Code>"; //char= 12
    string exitCode = "</style>"; // char = 8
    public TMP_Text text;

    public string confirmedColor = "<color=#000000>";

    public string usrColor = "<color=#FF0000>";

    //public string str;
    public List<char> keys = new List<char> {};
    public List<char> usrValues;
    public List<bool> valuesAssigned;

    public CipherDecode cipherDecode;

    void Start()
    {
        gameObject.AddComponent<CipherDecode>();
        cipherDecode = gameObject.GetComponent<CipherDecode>();

        //usrValues = new List<char>();

        usrValues = cipherDecode.GetUsrValues();
        valuesAssigned = cipherDecode.GetValuesAssigned();

        string str = text.text;

        text.text = Translate(str, keys);
    }

    public String Translate(string text, List<char> keys)
    {
        //changing the message to a StringBuilder to adjust based on index and putting all the characters into an array
        StringBuilder message = new StringBuilder(text);
        text.ToLower();
        char[] characters = text.ToCharArray();

        string exitRedCode = exitCode + usrColor;
        string exitBlackCode = exitCode + confirmedColor;
        string colorCode = code + "<color=#FFFFFF>";

        //keeps track of how many times something has been inserted into the message
        int hit = 0;
        //keeps track of how many characters have been added to the message
        int adjust = 0;

        usrValues = cipherDecode.GetUsrValues();
        valuesAssigned = cipherDecode.GetValuesAssigned();

        //looping through the total characters
        for (int i = 0; i < characters.Length; i++)
        {

            //a = 0 and z = 25
            if (characters[i] >= 'a' && characters[i] <= 'z' && characters[i] - 'a' < 25 && valuesAssigned[characters[i] - 'a'])
            {
                message[i + adjust] = usrValues[characters[i] - 'a'];

                message.Insert(i + (hit * (exitBlackCode.Length + colorCode.Length)), exitRedCode);
                adjust += exitRedCode.Length;

                Debug.Log("Message adjusted!");
                //message[i + adjust] = usrValues[(char)(characters[i] - 'a' + 1)];

                message.Insert(i + (exitBlackCode.Length + 1) + (hit * (exitBlackCode.Length + colorCode.Length)), colorCode);
                adjust += colorCode.Length;


                hit++;

            }

            //adding the index where a letter matches one of the keys
            else if (keys.Contains(characters[i]))
            {
                //inserting exitCode and Code at the adjusted indexes
                //if (usrValues.Contains(characters[i]))


                // {
                //     message.Insert(i + (hit * (exitBlackCode.Length + colorCode.Length)), exitRedCode);
                //     message.Insert(i + (exitBlackCode.Length + 1) + (hit * (exitBlackCode.Length + colorCode.Length)), colorCode);
                // }
                //else
                //{
                message.Insert(i + (hit * (exitBlackCode.Length + colorCode.Length)), exitBlackCode);
                adjust += exitBlackCode.Length;

                message.Insert(i + (exitBlackCode.Length + 1) + (hit * (exitBlackCode.Length + colorCode.Length)), colorCode);
                adjust += colorCode.Length;
                //}


                //increment to keep up with how many times an item was added 
                hit++;
            }

        }
        // setting total hits to 0 to avoid other indexes spilling over
        hit = 0;
        // adding code to the begining so everything is "encrypted"
        message.Insert(0, code);
        Debug.Log(keys);
        return message.ToString();
    }

}
