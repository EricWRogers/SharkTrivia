using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Text, Button, etc.
using TMPro; // If using TextMeshPro for text

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel; // Assign your pop-up UI panel in the Inspector
    public TextMeshProUGUI popupTitleText; // Assign your pop-up title TextMeshProUGUI
    public TextMeshProUGUI popupMessageText; // Assign your pop-up message TextMeshProUGUI
    public Button closeButton; // Assign your pop-up close button

    void Start()
    {
        // Ensure the pop-up is hidden at the start of the scene
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        // Add a listener to the close button
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePopup);
        }
    }

    // Call this method to show the pop-up with custom content
    public void ShowPopup(string title, string message)
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }

        if (popupTitleText != null)
        {
            popupTitleText.text = title;
        }

        if (popupMessageText != null)
        {
            popupMessageText.text = message;
        }
    }

    // Call this method to hide the pop-up
    public void ClosePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }
}