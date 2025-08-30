using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    public GameObject dialoguePanel;
    public Image portraitImage;
    public TMP_Text dialogueText, nameText;


    void Awake()
    {
        //make sure there is only one dialogue controller
        if (Instance == null) Instance = this;
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
        dialogueText.text = text; 
    }

}

