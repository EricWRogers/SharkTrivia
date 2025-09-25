using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DialogueManagerIntegrated : MonoBehaviour
{
    public static DialogueManagerIntegrated Instance { get; private set; }
    public static Translator translator;
    void Awake()
    {
        Instance = this;
        gameObject.AddComponent<Translator>();
        translator = gameObject.GetComponent<Translator>();
    }

    

    Conversation active;
    DNode current;

    public void StartConversation(Conversation convo)
    {
        if (!convo || !convo.entry) return;
        active = convo;
        ShowNode(convo.entry);
    }

    void ShowNode(DNode node)
    {
        current = node;

        // UI on
        var ui = DialogueController.Instance;
        ui.ShowDialogueUI(true);
        ui.SetCharInfo(node.speakerName, node.portrait);
        Debug.Log(node.speakerLine);
        ui.SetDialogueText(translator.Translate(node.speakerLine ,new List<char> { 'w', 'h', 'o', 'a', 'y','e' })); //temp change to encode
        ui.ClearChoices();

        if (node.choices != null && node.choices.Length > 0)
        {
            foreach (var c in node.choices)
            {
                var choiceCopy = c;
                ui.CreateChoiceButton(
                    translator.Translate(choiceCopy.choiceText ,new List<char> { 'w', 'h', 'o', 'a', 'y','e' }),
                    //TempCipherEncoder.Apply(choiceCopy.choiceText), // cipher choices too
                    () => OnChoiceSelected(choiceCopy)
                );
            }
            return;
        }

        // Branching
        if (node.choices != null && node.choices.Length > 0)
        {
            foreach (var c in node.choices)
            {
                var next = c.next; // capture
                ui.CreateChoiceButton(c.choiceText, () => ShowNode(next));
            }
            return;
        }

        // Linear
        if (node.autoProgress && node.nextIfNoChoices)
        {
            StopAllCoroutines();
            StartCoroutine(AutoNext(node.nextIfNoChoices, node.autoDelay));
            return;
        }

        // Manual continue
        ui.CreateChoiceButton("Continue", () =>
        {
            if (node.nextIfNoChoices) ShowNode(node.nextIfNoChoices);
            else EndConversation();
        });
    }

    void OnChoiceSelected(Choice c)
    {
        //tell player if correct or incorrect first
        if (c.isCorrect)
        {
            // Show a quick correct
            DialogueController.Instance.ClearChoices();
            DialogueController.Instance.SetDialogueText("Correct!");
            
            // Load a scene if needed
            if (!string.IsNullOrEmpty(c.loadSceneOnSelect))
            {
                StopAllCoroutines();
                StartCoroutine(AutoLoadScene(c.loadSceneOnSelect, 1.0f));
                return;
            }
        }
        else
        {
            //show as incorrect
            DialogueController.Instance.ClearChoices();
            DialogueController.Instance.SetDialogueText("Incorrect.");
        }

        // Continue to next node if present; otherwise end
        if (c.next != null) ShowNode(c.next);
        else EndConversation();
    }

    System.Collections.IEnumerator AutoLoadScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator AutoNext(DNode next, float delay)
    {
        yield return new WaitForSeconds(Mathf.Max(0f, delay));
        ShowNode(next);
    }

    public void EndConversation()
    {
        DialogueController.Instance.ShowDialogueUI(false);
        active = null;
        current = null;
        StopAllCoroutines();
    }
}
