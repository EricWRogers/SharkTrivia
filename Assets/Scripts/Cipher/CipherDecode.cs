using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    public enum GameMode { Easy, Medium, Hard }

    public GameMode difficulty = GameMode.Easy;

    //test build switches and trackers
    public bool isCountingGuesses = false;
    public int maxGuesses = 6;    
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
        {'a', '~'},{'b', '~'},{'c', '~'},{'d', '~'},
        {'e', '~'},{'f', '~'},{'g', '~'},{'h', '~'},
        {'i', '~'},{'j', '~'},{'k', '~'},{'l', '~'},
        {'m', '~'},{'n', '~'},{'o', '~'},{'p', '~'},
        {'q', '~'},{'r', '~'},{'s', '~'},{'t', '~'},
        {'u', '~'},{'v', '~'},{'w', '~'},{'x', '~'},
        {'y', '~'},{'z', '~'}
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

        /// <summary>
        /// prevents cipher from being deleted when a new level loads and other setup stuff.
        /// </summary>


        if (instance == null)
        {
            if (isRandomizing)
                RandomizeLetters();
            else
                UnRandomizeLetters();

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            Debug.Log("Singleton init");
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void clearUserLetters()
    {
        Debug.Log("Reseting cipher...");

        for (char i = 'a'; i < 'z'; i++)
        {
            charAssignments[i] = '~';
        }

        updateDisplays();
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
                

                if (isCountingGuesses && !(difficulty == GameMode.Medium && i == value))
                    numGuesses++;
                if(isLimitingCorrectness && i == value)
                    numCorrectGuesses++;

                if(isCountingGuesses && difficulty == GameMode.Medium && i == value)
                    confirmedCharAssignments[key] = value;
                else
                    charAssignments[key] = value;

                break;
            }

            //This if handles the case where the value that goes to the key in question is not blank but the new value also isn't found anywhere else in the journal
            else if (charAssignments[(char)i] != '~' && (char)i == key && !charAssignments.ContainsValue(value))
            {
                //Return an error code (some negative number) if you want this operation to be illegal, elsewise just overwrite the value if you're ok with it

                //returnCode = -1;

                //OR

                if (isCountingGuesses && !(difficulty == GameMode.Medium && i == value))
                    numGuesses++;
                

                if(isLimitingCorrectness && i == value)
                    numCorrectGuesses++;

                Debug.Log("CipherDecode: Overwrote previous char assignment");

                charAssignments[key] = '~';

                if(isCountingGuesses && difficulty == GameMode.Medium && i == value)
                    confirmedCharAssignments[key] = value;

                else
                    charAssignments[key] = value;

                break;
            }

            //This if handles the case where the new value is already used somehwere else in the journal with some subcases
            else if ((char)i == key && charAssignments.ContainsValue(value))
            {
                //Return a warning code (some other negative number) if you want this operation to be illegal, elsewise erase where the new value already was and put it here

                //Case where the value that goes to the key in question is blank
                // if (charAssignments[(char)i] == '~')
                // {
                //     Debug.Log("CipherDecode: return warning code -2");
                //     returnCode = -2;
                // }

                //Case where the value that goes to the key in question is not blank
                // if (charAssignments[(char)i] != '~')
                // {
                //     Debug.Log("CipherDecode: return warning code -3");
                //     returnCode = -3;
                // }

                //OR

                //var firstKey = charAssignments.FirstOrDefault(kvp => kvp.Value == value).Key;

                charAssignments[(char)i] = '~';
                charAssignments[key] = value;

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

    /// <summary>
    /// Handles enabling and disabling parts of the scene in accordance with the diffuclty setting. Should not be called directly, use UpdateGameModeHelper() instead.
    /// </summary>
    public void UpdateGameMode()
    {
        GameObject[] timerStuffs = GameObject.FindGameObjectsWithTag("Timer");

        switch (difficulty)
        {

            case GameMode.Hard:
                foreach (GameObject obj in timerStuffs)
                {
                    obj.SetActive(true);
                }

                break;

            default:
                foreach (GameObject obj in timerStuffs)
                {
                    obj.SetActive(false);
                }

                break;

        }
    }
    
    //for weird scriptable object reasons, I believe this should be called by dialogue nodes instead of UpdateGameMode() itself
    public void UpdateGameModeHelper()
    {
        instance.UpdateGameMode();
    }

    /// <summary>
    /// Changes the trivia game mode. 0 is easy, 1 is medium, and 2 is hard. Should not be called mid-trivia game.
    /// </summary>
    /// <param name="mode">The mode to change to (0-2).</param>
    public void ChangeGameMode(int mode)
    {
        difficulty = (GameMode)mode;

        // clearing the default letters
        for(char i = 'a'; i < 'z'; i++)
            confirmedCharAssignments[i] = '~';

        switch (difficulty)
        {
            case GameMode.Easy:

                isLimitingCorrectness = true;
                isCountingGuesses = false;

                // who ate ryan
                confirmedCharAssignments['w'] = 'w';
                confirmedCharAssignments['h'] = 'h';
                confirmedCharAssignments['o'] = 'o';
                confirmedCharAssignments['a'] = 'a';
                confirmedCharAssignments['t'] = 't';
                confirmedCharAssignments['e'] = 'e';
                confirmedCharAssignments['r'] = 'r';
                confirmedCharAssignments['y'] = 'y';
                confirmedCharAssignments['n'] = 'n';

                break;

            case GameMode.Medium:

                // vowels
                confirmedCharAssignments['a'] = 'a';
                confirmedCharAssignments['e'] = 'e';
                confirmedCharAssignments['i'] = 'i';
                confirmedCharAssignments['o'] = 'o';
                confirmedCharAssignments['u'] = 'u';
                
                isLimitingCorrectness = false;
                isCountingGuesses = true;
                break;

            case GameMode.Hard:

                // no default letters
                isLimitingCorrectness = false;
                isCountingGuesses = true;
                break;

        }

        numCorrectGuesses = 0;
        numGuesses = 0;

        updateDisplays();
        clearUserLetters();
    }


    /// <summary>
    /// Changes the number of guesses at letters the player is allowed to make.
    /// </summary>
    /// <param name="change">The number to modify the guesses by (negative subtracts guesses, positive adds them).</param>
    public void changeGuessNum(int change)
    {
        if(isCountingGuesses)
            maxGuesses += change;
        else if(isLimitingCorrectness)
            maxCorrectGuesses += change;
    }

    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        if(next.name.ToLower().Contains("trivia"))
            UpdateGameMode();
    }
    
}
