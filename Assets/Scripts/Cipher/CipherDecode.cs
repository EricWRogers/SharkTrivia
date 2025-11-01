using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//Originally Programmed by Samuel (Scott)

public class CipherDecode : MonoBehaviour
{
    [System.Serializable]
    public class CipherNode { public char key; public char value; }
    public static CipherDecode instance = null;
    public List<CipherNode> charAssignmentDisplay;
    public List<CipherNode> secondOrderDisplay;
    public List<CipherNode> confirmedAssignmentDisplay;
    public bool encoding = true;
    public bool isRandomizing = false;

    //test build switches and trackers
    public bool isCountingGuesses = false;
    public int maxGuesses = 4;    
    public int numGuesses = 0;


    public bool isLimitingCorrectness = false;
    public int maxCorrectGuesses = 4;
    public int numCorrectGuesses = 0;
    
    //

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

    public Dictionary<char, char> confirmedCharAssignments = new Dictionary<char, char>
    {
        //tilde represents an english character which has not been assigned a ciphertext equivalent 
        {'a', 'a'},{'b', '~'},{'c', '~'},{'d', '~'},
        {'e', 'e'},{'f', '~'},{'g', '~'},{'h', 'h'},
        {'i', '~'},{'j', '~'},{'k', '~'},{'l', '~'},
        {'m', '~'},{'n', 'n'},{'o', 'o'},{'p', '~'},
        {'q', '~'},{'r', 'r'},{'s', '~'},{'t', 't'},
        {'u', '~'},{'v', '~'},{'w', '~'},{'x', '~'},
        {'y', 'y'},{'z', '~'}
    };

    public Dictionary<char, char> secondOrderAssoc = new Dictionary<char, char>
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
            if (isRandomizing)
                RandomizeLetters();
            else
                UnRandomizeLetters();


            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(this);
            return;
        }

    }

    void updateDisplays()
    {
        charAssignmentDisplay.Clear();

        foreach (char k in charAssignments.Keys)
        {
            charAssignmentDisplay.Add(new CipherNode { key = k, value = charAssignments[k] });
        }


        secondOrderDisplay.Clear();

        foreach (char k in secondOrderAssoc.Keys)
        {
            secondOrderDisplay.Add(new CipherNode { key = k, value = secondOrderAssoc[k] });
        }

        confirmedAssignmentDisplay.Clear();

        foreach (char k in confirmedCharAssignments.Keys)
        {
            confirmedAssignmentDisplay.Add(new CipherNode { key = k, value = confirmedCharAssignments[k] });
        }
    }

    void RandomizeLetters()
    {
        List<char> letters = new List<char> {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                                             'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
        int n = letters.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1); // UnityEngine.Random
            (letters[k], letters[n]) = (letters[n], letters[k]);
        }

        for (int i = 'a'; i <= 'z'; i++)
        {
            secondOrderAssoc[(char)i] = letters[i - 97];
        }

        updateDisplays();

    }

    void UnRandomizeLetters()
    {
        for (int i = 'a'; i <= 'z'; i++)
        {
            secondOrderAssoc[(char)i] = (char)i;
        }


        updateDisplays();
    }

    public List<char> GetUsrValues()
    {
        List<char> usrValues = new List<char>();

        for (int i = 'a'; i <= 'z'; i++)
        {
            usrValues.Add(charAssignments[(char)i]);
        }


        return usrValues;
    }

    public List<bool> GetUsrValuesAssigned()
    {
        List<bool> valsAssigned = new List<bool>();

        for (int i = 'a'; i <= 'z'; i++)
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

        for (int i = 'a'; i <= 'z'; i++)
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
            //Debug.Log("Checking character: " + (char)i);

            //This if handles the case where the value that goes to the key in question is blank ('~') and the new value is not already found elsehwere
            if (charAssignments[(char)i] == '~' && (char)i == key && !charAssignments.ContainsValue(value))
            {
                charAssignments[key] = value;

                if (isCountingGuesses)
                {
                    numGuesses++;
                }
                if(isLimitingCorrectness && i == value)
                {
                    numCorrectGuesses++;
                }


                break;
            }

            //This if handles the case where the value that goes to the key in question is not blank but the new value also isn't found anywhere else in the journal
            else if (charAssignments[(char)i] != '~' && (char)i == key && !charAssignments.ContainsValue(value))
            {
                //Return an error code (some negative number) if you want this operation to be illegal, elsewise just overwrite the value if you're ok with it

                //returnCode = -1;

                //OR

                if (isCountingGuesses)
                {
                    numGuesses++;
                }
                if(isLimitingCorrectness && i == value)
                {
                    numCorrectGuesses++;
                }

                Debug.Log("CipherDecode: Overwrote previous char assignment");
                charAssignments[key] = value;
                break;
            }

            //This if handles the case where the new value is already used somehwere else in the journal with some subcases
            else if ((char)i == key && charAssignments.ContainsValue(value))
            {
                //Return an error code (some other negative number) if you want this operation to be illegal, elsewise erase where the new value already was and put it here

                //Case where the value that goes to the key in question is blank
                if (charAssignments[(char)i] == '~')
                {
                    Debug.Log("CipherDecode: return warning code -2");
                    returnCode = -2;
                }

                //Case where the value that goes to the key in question is not blank
                if (charAssignments[(char)i] != '~')
                {
                    Debug.Log("CipherDecode: return warning code -3");
                    returnCode = -3;
                }

                //OR

                // var firstKey = charAssignments.FirstOrDefault(kvp => kvp.Value == value).Key;
                // charAssignments[firstKey] = '~';
                // charAssignments[key] = value;

                break;
            }
            //This handles the case where the player hits a letter they already did for this cipher character, so it'll just dissasociate it and go back to being blank
            else if (charAssignments[(char)i] != '~' && (char)i == key && !charAssignments.ContainsValue(value) && charAssignments[key] == value)
            {
                Debug.Log("CipherDecode: erasing previous assignment");
                charAssignments[key] = '~';
                break;
            }

        }


        updateDisplays();

        return returnCode;

    }
    public int ConfirmedCharAssignment(char key, char value)
    {
        confirmedCharAssignments[key] = value;
        updateDisplays();
        return 1;

    }
    
}
