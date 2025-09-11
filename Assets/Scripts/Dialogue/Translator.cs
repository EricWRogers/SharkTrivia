using UnityEngine;
using TMPro;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;


public class Translator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    string code = "<style=Code>"; //char= 12
    string exitCode = "</style>"; // char = 8
    public TMP_Text text;
    public string str;
    public List<char> keys = new List<char>();

    //SCOTT ADDED
    public GameObject usrDecode;
     List<char> usrKeys = new List<char>();
    //END SCOTT ADDED

    int offset = 20; //total characters being added every loop
    void Start()
    {
        //SCOTT ADDED
        usrKeys = usrDecode.GetComponent<CipherDecode>().keys;
        //END SCOTT ADDED
        
        text.text = Translate(str, keys);
    }

    String Translate(string text, List<char> keys)
    {
        //changing the message to a StringBuilder to adjust based on index and putting all the characters into an array
        StringBuilder message = new StringBuilder(text);
        char[] characters = text.ToCharArray();

        string exitBlackCode = exitCode + "<color=#000000>";
        string exitRedCode = exitCode + "<color=#FF0000>";
        string colorCode = code + "<color=#FFFFFF>";

        //keeps track of how many times something has been inserted into the message
        int hit = 0;
        //looping through the total characters
        for (int i = 0; i < characters.Length; i++)
        {

            //adding the index where a letter matches one of the keys
            if (keys.Contains(characters[i]) || usrKeys.Contains(characters[i]))
            {
                //inserting exitCode and Code at the adjusted indexes
                if (usrKeys.Contains(characters[i]))
                {
                    message.Insert(i + (hit * (exitBlackCode.Length + colorCode.Length)), exitRedCode);
                    message.Insert(i + (exitBlackCode.Length + 1) + (hit * (exitBlackCode.Length + colorCode.Length)), colorCode);
                }
                else
                {
                    message.Insert(i + (hit * (exitBlackCode.Length + colorCode.Length)), exitBlackCode);
                    message.Insert(i + (exitBlackCode.Length + 1) + (hit * (exitBlackCode.Length + colorCode.Length)), colorCode);
                }


                //increment to keep up with how many times an item was added 
                hit++;
            }

        }
        // setting total hits to 0 to avoid other indexes spilling over
        hit = 0;
        // adding code to the begining so everything is "encrypted"
        message.Insert(0, code);
        return message.ToString();
    }

}
