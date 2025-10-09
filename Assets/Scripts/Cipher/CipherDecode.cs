using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

// //Originally Programmed by Samuel (Scott)

public class CipherDecode : MonoBehaviour
{
    [System.Serializable]
    public class CipherNode { public char key; public char value; }
    public static CipherDecode instance = null;
    public List<CipherNode> charAssignmentDisplay;
    public bool encoding = true;

    private Dictionary<char, char> charAssignments = new Dictionary<char, char>
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

    public Dictionary<char, char> confirmedCharAssignments = new Dictionary<char, char>
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

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    public List<char> GetUsrValues()
    {
        List<char> usrValues = new List<char>();

        for (int i = 'a'; i < 'z'; i++)
        {
            usrValues.Add(charAssignments[(char)i]);
        }


        return usrValues;
    }

    public List<bool> GetUsrValuesAssigned()
    {
        List<bool> valsAssigned = new List<bool>();

        for (int i = 97; i < 122; i++)
        {
            if (charAssignments[(char)i] != '~')
            {
                valsAssigned.Add(true);
            }
            else
            {
                valsAssigned.Add(false);
            }
        }

        return valsAssigned;
    }

    public List<char> GetConfirmedChars()
    {
        List<char> confirmedChars = new List<char>();

        for (int i = 'a'; i < 'z'; i++)
        {
            if (confirmedCharAssignments[(char)i] != '~')
            {
                confirmedChars.Add(confirmedCharAssignments[(char)i]);
            }
        }

        return confirmedChars;
    }

    //pass in the key and value you are trying to edit
    public int CharAssignment(char key, char value)
    {
        key = key.ToString().ToLower()[0];
        value = value.ToString().ToLower()[0];
        int returnCode = 0;

        for (int i = 'a'; i <= 'z'; i++)
        {
            Debug.Log("Checking character: " + (char)i);

            //This if handles the case where the value that goes to the key in question is blank ('~') and the new value is not already found elsehwere
            if (charAssignments[(char)i] == '~' && (char)i == key && !charAssignments.ContainsValue(value))
            {
                charAssignments[key] = value;
                break;
            }

            //This if handles the case where the value that goes to the key in question is not blank but the new value also isn't found anywhere else in the journal
            else if (charAssignments[(char)i] != '~' && (char)i == key && !charAssignments.ContainsValue(value))
            {
                //Return an error code (some negative number) if you want this operation to be illegal, elsewise just overwrite the value if you're ok with it

                returnCode = -1;

                //OR

                // charAssignments[key] = value;
                break;
            }

            //This if handles the case where the new value is already used somehwere else in the journal with some subcases
            else if ((char)i == key && charAssignments.ContainsValue(value))
            {
                //Return an error code (some other negative number) if you want this operation to be illegal, elsewise erase where the new value already was and put it here

                //Case where the value that goes to the key in question is blank
                if (charAssignments[(char)i] == '~')
                {
                    returnCode = -2;
                }

                //Case where the value that goes to the key in question is not blank
                if (charAssignments[(char)i] != '~')
                {
                    returnCode = -3;
                }

                //OR

                // var firstKey = charAssignments.FirstOrDefault(kvp => kvp.Value == value).Key;
                // charAssignments[firstKey] = '~';
                // charAssignments[key] = value;

                break;
            }
            //This handles the case where the player hits a letter they already did for this cipher character, so it'll just dissasociate it and go back to being blank
            else if (charAssignments[(char)i] == '~' && (char)i == key && !charAssignments.ContainsValue(value) && charAssignments[key] == value)
            {
                charAssignments[key] = '~';
                break;
            }

        }


        charAssignmentDisplay.Clear();

        foreach (char k in charAssignments.Keys)
        {
            charAssignmentDisplay.Add(new CipherNode { key = k, value = charAssignments[k] });
        }


        return returnCode;

    }
    public int ConfirmedCharAssignment(char key, char value)
    {
        confirmedCharAssignments[key] = value;
        return 1;

    }
    
}
