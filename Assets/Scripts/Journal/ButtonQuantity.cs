/*using UnityEngine;
using UnityEngine.UI;

public class ButtonQuantity : MonoBehaviour
{
    public GameObject GuessButton;
    public Transform JournalBackground;
    public TMP_Text yourText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject newButton = Instantiate(GuessButton, JournalBackground);

        Button buttonComponent = newButton.GetComponent<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(OnButtonClick);
        }


        Text buttonText = newButton.GetComponentInChildren<text>();
        if (buttonText != null)
        {
            buttonText.yourText = "Dynamic Button";
        }
    }

    // Update is called once per frame
    void OnButtonClick()
    {
        Debug.Log("Button clicked");
    }
}*/
