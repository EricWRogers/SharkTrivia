using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour, IInteractable
{
    public CharDialogue dialogueData;
    private DialogueController dialogueController;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    private void Start()
    {
        dialogueController = DialogueController.Instance;
        //isDialogueActive = true;
    }


    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {

        if (dialogueData == null)
        {
            return;
        }

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    } 

    public void StartDialogue()
    {
        Debug.Log("Start Dialogue");
        isDialogueActive = true;
        dialogueIndex = 0;

        dialogueController.SetCharInfo(dialogueData.charName, dialogueData.charPortrait);
        dialogueController.ShowDialogueUI(true);

        DisplayCurrentLine();
    }

    void NextLine()
    {
        //Debug.Log("Current index: " + dialogueIndex);
        Debug.Log("Next line");

        if (isTyping)
        {
            //skip typing animation and show the full line
            StopAllCoroutines();
            dialogueController.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        //Clear existing choices
        dialogueController.ClearChoices();

        //Check endDialogueLines
        if(dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }
        //Check if choices exist & if true display them
        foreach(DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if(dialogueChoice.dialogueIndex == dialogueIndex)
            {
                //display choices
                DisplayChoices(dialogueChoice);
            }
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        Debug.Log("type line");

        isTyping = true;
        dialogueController.SetDialogueText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueController.SetDialogueText(dialogueController.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        isTyping = false;

        if(dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    void DisplayChoices(DialogueChoice choice)
    {
        Debug.Log("Display Choices");

        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            dialogueController.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex));
        }
    }

    void ChooseOption(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueController.ClearChoices();
        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        Debug.Log("Display current line");
        Debug.Log("Current index: " + dialogueIndex);

        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        Debug.Log("end dialogue");
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueController.SetDialogueText("");
        dialogueController.ShowDialogueUI(false);
    }
}
