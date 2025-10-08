using UnityEngine;

public class GuessLetterMessager : MonoBehaviour
{
    public ButtonRename buttonRename;
    public string letter;

    public void Open()
    {
        buttonRename.Open(gameObject.transform.GetChild(0).gameObject, letter);
    }
}
