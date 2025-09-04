using UnityEngine;
using TMPro;
using System.Text;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using UnityEditor.SceneManagement;

public class Translator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    string code = "<style=Code>"; //char= 12
    string exitCode = "</style>"; // char = 8
    public TMP_Text text;
    public string str;
    List<char> keys = new List<char>();
    List<int> hits = new List<int>();

    int offset = 20;
    // because exitCode is added first it offsets the indexes for code
    int adjustedCharacters = 9;
    void Start()
    {
        keys.Add('e');
        keys.Add('d');
        keys.Add('y');
        keys.Add('a');
        keys.Add('r');

        text.text = Translate(str, keys);
    }

    String Translate(string text, List<char> keys)
    {
        //changing the message to a StringBuilder to adjust based on index and putting all the characters into an array
        StringBuilder message = new StringBuilder(text);
        char[] characters = text.ToCharArray();

        //looping through the total characters
        for (int i = 0; i < characters.Length; i++)
        {
            //looping through the key
            for (int x = 0; x < keys.Count; x++)
            {
                //adding the index where a letter matches one of the keys
                if (characters[i] == keys[x])
                {
                    hits.Add(i);
                }
            }
        }

        //looping through the indexes that had a key letter
        for (int y = 0; y < hits.Count; y++)
        {
            // this is the index of a key letter in the orignal text itself
            Debug.Log($"target index: {hits[y]}");

            // this is the actual index because it changes based on how many times the tags were added
            Debug.Log($"actual indexes: {hits[y] - (y * offset)},  {hits[y] + (y * offset) + adjustedCharacters}");

            message.Insert(hits[y] + (y * offset), exitCode);
            message.Insert(hits[y] + (y * offset) + adjustedCharacters, code);
        }
        // clearing so theres no chance leftover indexes can stick around and adding the tag to encode everything at the beggining of the string
        hits.Clear();
        message.Insert(0, code);
        return message.ToString();
    }
}
