using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GuessLetterMessager : MonoBehaviour
{
    public ButtonRename buttonRename;
    public Translator translator;
    public string letter;

    public void Open()
    {
        buttonRename.Open(gameObject.transform.GetChild(0).gameObject, letter);
    }

    void OnEnable()
    {
        if (CipherDecode.instance.confirmedCharAssignments.ContainsValue(letter.ToLower()[0]))
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    } 
}
