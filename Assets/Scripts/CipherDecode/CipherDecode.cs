using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

//Originally Programmed by Samuel (Scott)

public class CipherDecode : MonoBehaviour
{

    public GameObject alphabetButtons = null;
    //list of all the children of the GameObject "AlphabetButtons"
    List<GameObject> cipherButtonsGroups = new List<GameObject>();
    List<GameObject> cipherButtons = new List<GameObject>();

    //needs to be public because Translator.cs (or another script?) will need to reference this dictionary
    public static Dictionary<char, char> charAssignments = new Dictionary<char, char>
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



    void Start()
    {
        for(int i = 0; i < alphabetButtons.transform.childCount; i++)
            cipherButtonsGroups.Add(alphabetButtons.transform.GetChild(i).gameObject);


        for (int j = 0; j < cipherButtonsGroups.Count; j++)
        {
            for (int i = 0; i < cipherButtonsGroups[j].transform.childCount; i++)
            {
                cipherButtons.Add(cipherButtonsGroups[j].transform.GetChild(i).gameObject);
            }
        }

        foreach (GameObject alphabetButton in cipherButtonsGroups)
        {
            foreach (Transform child in alphabetButton.transform)
            {
                Button btn = child.GetComponent<Button>();

                //"NullReferenceException" here for some reason                  <<< TO FIX!
                int buttonIndex = WhoAmI(child.gameObject);
                btn.onClick.AddListener(() => ExpandCollapse(buttonIndex));
            }
        }

        //this nested foreach creates a listener which later identifies which cipher text button in the cipherButton group was pressed
        foreach (GameObject alphabetButton in cipherButtonsGroups)
        {
            foreach (Transform child in alphabetButton.transform)
            {
                Button btn = child.GetComponent<Button>();

                int buttonIndex = WhoAmI(child.gameObject);
                btn.onClick.AddListener(() => CharAssignment(buttonIndex, child.GetChild(1).gameObject));
            }
        }
    }

    //place holder empty function
    public void SpeakUP()
    {

    }

    //"expands" or "collapses" the cipher text sub menu when the english character button is pressed
    public void ExpandCollapse(int buttonIndex)
    {
        //sets the cipherButtons' status to be the opposite of what it what it already was
        cipherButtonsGroups[buttonIndex].SetActive(!cipherButtonsGroups[buttonIndex].activeSelf);
    }


    //PURPOSE OF THIS FUNCTION: to go up a level to the parent object and return our ID (index) relative to the parent
    int WhoAmI(GameObject currentObject)
    {
        Transform parentTransform = currentObject.transform.parent;

        if (parentTransform == null)
        {
            Debug.LogError($"WhoAmI failed: {currentObject.name} has no parent.");
            return -1;
        }

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            if (parentTransform.GetChild(i).gameObject == currentObject)
            {
                return i;
            }
        }

        Debug.LogWarning($"WhoAmI: {currentObject.name} not found among siblings.");
        return -1;
    }


    //this function uses cipherButtonsGroup to find what english character we are in, then indexes down in the list of children of the cipherButtonsGroup to put that
    //together to change the dictionary values
    public void CharAssignment(int cipherButtonID, GameObject cipherButtonGroup)
    {
        int englishCharIndex;
        englishCharIndex = WhoAmI(cipherButtonGroup.transform.parent.gameObject);
        //0-25 corresponds to letters

        char englishChar = (char)((char)englishCharIndex + 97);
        char cipherButtonChar = (char)((char)cipherButtonID + 97);

        //this if else block handles assigning the characters in the dictionary or resetting them to '~' if the button is hit again
        if (charAssignments[cipherButtonChar] != englishChar)
            charAssignments[cipherButtonChar] = englishChar;
        else
            charAssignments[cipherButtonChar] = '~';

        Debug.Log(cipherButtonChar);

    }

}
