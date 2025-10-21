using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class TranslateGuessLetter : MonoBehaviour
{
    public Translator translator;
    public GameObject currentButton;
    public TMP_Text gButton;
    private List<string> defaultCharacters;
    private bool defaultYet = false;

    void OnEnable()
    {
        if (defaultYet == false)
        {
            defaultCharacters = new List<string>();
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                currentButton = gameObject.transform.GetChild(i).gameObject;
                defaultCharacters.Append(currentButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text);
            }        
        } 

        defaultYet = true;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            currentButton = gameObject.transform.GetChild(i).gameObject;
            gButton = currentButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            Debug.Log(i);
            gButton.text = translator.Translate(defaultCharacters[i]);
        }
    }
}