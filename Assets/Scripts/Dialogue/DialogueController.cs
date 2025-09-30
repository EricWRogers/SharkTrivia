using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEditor.Rendering;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    public GameObject dialoguePanel;
    public Image portraitImage;
    public TMP_Text dialogueText, nameText;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;


    //SCOTT CHANGES START
    private Translator translator;
    //SCOTT CHANGES END

    void Awake()
    {
        //make sure there is only one dialogue controller
        if (Instance == null)
        {
            Instance = this;

            //SCOTT CHANGES START
            gameObject.AddComponent<Translator>();
            translator = gameObject.GetComponent<Translator>();
            //SCOTT CHANGES END
        }
        else Destroy(gameObject);

    }

    //activate dialogue UI
    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show); //toggle UI visibilty
    }

    //Save character info
    public void SetCharInfo(string charName, Sprite portrait)
    {
        nameText.text = charName;
        portraitImage.sprite = portrait;
    }

    public void SetDialogueText(string text)
    {
        //SCOTT ADDED
        //text = translator.Translate(text, new List<char> { 'w', 'h', 'o', 'a', 'y','e' });
        //END SCOTT ADDED

        dialogueText.text = text; 
    }

    //choices
    public void ClearChoices()
    {
        foreach(Transform child in choiceContainer) Destroy(child.gameObject);
    }
    
    public GameObject CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
        return choiceButton;
    }
}

