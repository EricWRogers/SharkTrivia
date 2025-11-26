using UnityEngine;
using TMPro;
using System.Text;
using System.Collections.Generic;




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
            tr.DEBUG_TEXT.text = tr.Translate("the quick brown fox jumps over the lazy dog");
        }
    }
}

#endif

public class Translator : MonoBehaviour
{
    string code = "<style=Code>"; //char= 12
    string exitCode = "</style>"; // char = 8
    public TMP_Text DEBUG_TEXT;

    public string confirmedColor = "<color=#0000FF>";

    public string usrColor = "<color=#FF0000>";

    public string defaultColor = "<color=#000000>";

    public List<char> usrValues;
    public List<bool> valuesAssigned;


    public string Translate(string text)
    {
        List<char> keys = CipherDecode.instance.GetConfirmedChars();
        usrValues = CipherDecode.instance.GetUsrValues();


        valuesAssigned = CipherDecode.instance.GetUsrValuesAssigned();

        //changing the message to a StringBuilder to adjust based on index and putting all the characters into an array
        StringBuilder message = new StringBuilder(text.ToLower());


        List<int> charIndexes = new List<int>();
        
        //Sets letters equal to their second order associations before translating (turning this off is handled with a public Bool in CipherDecode.cs)
        for (int i = 0; i < message.Length && CipherDecode.instance.isRandomizing; i++)
        {
            if (char.IsLetter(message[i]) && !CipherDecode.instance.confirmedCharAssignments.ContainsValue(message[i]) && !CipherDecode.instance.charAssignments.ContainsValue(message[i]))
            {
                charIndexes.Add(i);
                message[i] = CipherDecode.instance.secondOrderAssoc[message[i]];
            }
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
            if (characters[i] >= 'a' && characters[i] <= 'z' && characters[i] - 'a' < 25 && valuesAssigned[characters[i] - 'a'] && !charIndexes.Contains(i))
            {
                message[i + adjust] = usrValues[characters[i] - 'a'];

                message.Insert(i + (hit * (exitBlackCode.Length + colorCode.Length)), exitRedCode);
                adjust += exitRedCode.Length;

                message.Insert(i + (exitBlackCode.Length + 1) + (hit * (exitBlackCode.Length + colorCode.Length)), colorCode);
                adjust += colorCode.Length;


                hit++;

            }

            else if ((keys.Contains(characters[i]) && !charIndexes.Contains(i)) || !char.IsLetter(characters[i]))
            {
                message.Insert(i + (hit * (exitBlackCode.Length + colorCode.Length)), exitBlackCode);
                adjust += exitBlackCode.Length;

                message.Insert(i + (exitBlackCode.Length + 1) + (hit * (exitBlackCode.Length + colorCode.Length)), colorCode);
                adjust += colorCode.Length;

                //increment to keep up with how many times an item was added 
                hit++;
            }

        }
        // adding code to the begining so everything is "encrypted"
        message.Insert(0, code);
        
        return message.ToString();
    }

}
