using UnityEngine;
using TMPro;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
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
            tr.usrValues = CipherDecode.instance.GetUsrValues();
            tr.valuesAssigned = CipherDecode.instance.GetUsrValuesAssigned();
            tr.text.text = tr.Translate("the quick brown fox jumps over the lazy dog");
        }
    }
}

#endif

public class Translator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    string code = "<style=Code>"; //char= 12
    string exitCode = "</style>"; // char = 8
    public TMP_Text text;

    public string confirmedColor = "<color=#0000FF>";

    public string usrColor = "<color=#FF0000>";

    public string defaultColor = "<color=#000000>";

    //public string str;
    //public List<char> keys = new List<char> { };
    public List<char> usrValues;
    public List<bool> valuesAssigned;

    //public CipherDecode cipherDecode;

    void Start()
    {
        //gameObject.AddComponent<CipherDecode>();
        //cipherDecode = gameObject.GetComponent<CipherDecode>();

        //usrValues = new List<char>();

        //usrValues = CipherDecode.instance.GetUsrValues();
        //valuesAssigned = CipherDecode.instance.GetUsrValuesAssigned();

        //string str = text.text;

        //text.text = Translate(str, keys);
    }

    public String Translate(string text)
    {
        List<char> keys = CipherDecode.instance.GetConfirmedChars();
        usrValues = CipherDecode.instance.GetUsrValues();


        valuesAssigned = CipherDecode.instance.GetUsrValuesAssigned();

        //changing the message to a StringBuilder to adjust based on index and putting all the characters into an array
        StringBuilder message = new StringBuilder(text.ToLower());

        //Sets letters equal to their second order associations before translating (turning this off is handled with a public Bool in CipherDecode.cs)
        for (int i = 0; i < message.Length; i++)
        {
            if (char.IsLetter(message[i]))
                message[i] = CipherDecode.instance.secondOrderAssoc[message[i]];
        }

        char[] characters = text.ToLower().ToCharArray();

        string exitRedCode = exitCode + usrColor;
        string exitBlackCode = exitCode + confirmedColor;
        string colorCode = code + defaultColor;

        //keeps track of how many times something has been inserted into the message
        int hit = 0;
        //keeps track of how many characters have been added to the message
        int adjust = 0;



        //looping through the total characters
        for (int i = 0; i < characters.Length; i++)
        {

            //a = 0 and z = 25
            if (characters[i] >= 'a' && characters[i] <= 'z' && characters[i] - 'a' < 25 && valuesAssigned[characters[i] - 'a'])
            {
                message[i + adjust] = usrValues[characters[i] - 'a'];

                message.Insert(i + (hit * (exitBlackCode.Length + colorCode.Length)), exitRedCode);
                adjust += exitRedCode.Length;

                //Debug.Log("Message adjusted!");
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
        //Debug.Log(keys);

        message = TransformStyledColorText(message);

        //Debug.Log(message.ToString());
        return message.ToString();
    }

    public StringBuilder TransformStyledColorText(StringBuilder sb)
    {
        string input = sb.ToString();
        var output = new StringBuilder();

        // Match <style=Code>...</style> blocks
        string stylePattern = @"<style=Code>(.*?)</style>";
        var styleMatches = Regex.Matches(input, stylePattern, RegexOptions.Singleline);

        int lastIndex = 0;

        foreach (Match styleMatch in styleMatches)
        {
            int start = styleMatch.Index;
            int end = styleMatch.Index + styleMatch.Length;

            // Append everything before this style block
            if (start > lastIndex)
                output.Append(input.Substring(lastIndex, start - lastIndex));

            string styledContent = styleMatch.Groups[1].Value;

            // Match <color=#XXXXXX> followed by non-tag text
            string colorPattern = $@"<color=({Regex.Escape(confirmedColor)}|{Regex.Escape(usrColor)})>([^<]*)";
            var transformed = Regex.Replace(styledContent, colorPattern, match =>
            {
                string color = match.Groups[1].Value;
                string text = match.Groups[2].Value;

                var map = color.Equals(confirmedColor, StringComparison.OrdinalIgnoreCase)
                    ? CipherDecode.instance.confirmedCharAssignments
                    : CipherDecode.instance.charAssignments;

                var transformedText = new StringBuilder();
                foreach (char c in text)
                {
                    if (char.IsLetter(c) && map.ContainsKey(c))
                    {
                        transformedText.Append(map[c]);
                    }
                    else
                        transformedText.Append(c);
                }

                return $"<color={color}>{transformedText}";
            });

            output.Append($"<style=Code>{transformed}</style>");
            lastIndex = end;
        }

        // Append any remaining content after the last style block
        if (lastIndex < input.Length)
            output.Append(input.Substring(lastIndex));

        return output;
    }





}
