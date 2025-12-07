using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public bool difficultyDebugMode = false;

    public bool isCountingGuesses = false;
    public int maxGuesses = 6;    
    public int numGuesses = 0;


    public bool isLimitingCorrectness = false;
    public int maxCorrectGuesses = 4;
    public int numCorrectGuesses = 0;

    public int additionalGuesses = 0;
    private int origCorrectGuesses;
    private int origGuesses;

    public int numOfRandLetters = 5;
    public List<bool> unassButtonsToEnable;
    

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

            unassButtonsToEnable = new List<bool>{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
                                                  false,false,false,false,false,false,false,false};

            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            if(difficultyDebugMode)
            {
                UpdateGameMode();
                ChangeGameMode((int)difficulty);
            }

            origCorrectGuesses = maxCorrectGuesses;
            origGuesses = maxGuesses;

            // jank
            maxCorrectGuesses -= additionalGuesses;
            maxGuesses -= additionalGuesses;

            Debug.Log("Singleton init");
            instance = this;
            DontDestroyOnLoad(gameObject);

            // handling journal U.I. stuff

            GuessLetterMessager[] cipherButtons = FindObjectsByType<GuessLetterMessager>(FindObjectsSortMode.None);

            foreach (GuessLetterMessager butt in cipherButtons)
            {
                butt.OnJournalEnter();
            }

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
        int returnCode = 1;
        

        Debug.Log($"Just got passed Key: {key} to Value: {value}");

        // handles erasing a value
        if(value == '~')
        {
            int charIndex = key - 'a';

            //instance.charAssignments[key] = value;

            char prevVal = '?'; // should never end as '?'

            // which dictionary key was assigned to the name of the key parameter?
            for(char i = 'a'; i < 'z'; i++)
            {
                if(charAssignments[i] == key)
                {
                    prevVal = i;
                }
            }

            instance.charAssignments[prevVal] = value;

            unassButtonsToEnable[charIndex] = false;

            Debug.Log($"Just erased Key: {prevVal} to Value: {value}");

            returnCode = -1;
        }
        
        // handles adding a new value
        else if(!charAssignments.ContainsValue(value))
        {
            int charIndex = value - 'a';

            if(isCountingGuesses)
                numGuesses++;
            if(isLimitingCorrectness && key == value)
                numCorrectGuesses++;

            unassButtonsToEnable[charIndex] = true;

            Debug.Log($"Just assigned Key: {key} to Value: {value}");
            instance.charAssignments[key] = value;
            returnCode = 0;
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
        {
            confirmedCharAssignments[i] = '~';
            charAssignments[i] = '~';
        }

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

                // random selection of letters
                isLimitingCorrectness = false;
                isCountingGuesses = true;

                List<char> listOfChars = new List<char> {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                                             'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
                int randNum;

                for(int i = 0; i < numOfRandLetters; i++)
                {
                    randNum = Random.Range(0, listOfChars.Count);

                    confirmedCharAssignments[listOfChars[randNum]] = listOfChars[randNum];

                    listOfChars.RemoveAt(randNum);
                }

                break;

        }

        if(!difficultyDebugMode)
            isRandomizing = GameObject.Find("RandomCheckBox").GetComponent<Toggle>().isOn;

        if (isRandomizing)
            RandomizeLetters();
        else
            UnRandomizeLetters();


        numCorrectGuesses = 0;
        numGuesses = 0;

        if(!difficultyDebugMode)
        {
            maxCorrectGuesses = origCorrectGuesses;
            maxGuesses = origGuesses;
        }
        
        clearUserLetters();
        updateDisplays();
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
        {
            UpdateGameMode();
            changeGuessNum(additionalGuesses);
            numGuesses = 0;
            numCorrectGuesses = 0;
        }
    }
    
}
