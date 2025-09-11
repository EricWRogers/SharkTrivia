using UnityEngine;
using TMPro;
using UnityEngine.UI;


/// A simple UI bubble that shows speaker name, line text, and a vertical list of thee choice buttons.
/// This class is presentation only, it doesn't decide which node to show next, that's in the manager
public class BackStageDialogueBubble : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI bodyText;
    public Transform choicesContainer;
    public Button choiceButtonTemplate;

    void Awake()
    {
        gameObject.SetActive(false);
        if (choiceButtonTemplate) choiceButtonTemplate.gameObject.SetActive(false);
    }


 /// Display the bubble with a speaker name and a single line of text.
    /// Existing dynamic choice buttons are cleared.
    public void Show(string speaker, string line)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        nameText.text = speaker;
        bodyText.text = line;
        ClearChoices();
    }

 /// Destroys all previous choice buttons
    public void ClearChoices()
    {
        for (int i = choicesContainer.childCount - 1; i >= 0; i--)
            if (choicesContainer.GetChild(i) != choiceButtonTemplate.transform)
                Destroy(choicesContainer.GetChild(i).gameObject);
    }

    /// Creates a clickable choice button from the template.
    public void AddChoice(string text, System.Action onClick)
    {
        var btn = Instantiate(choiceButtonTemplate, choicesContainer);
        btn.gameObject.SetActive(true);
        btn.GetComponentInChildren<TextMeshProUGUI>().text = text;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => onClick?.Invoke());
    }

/// Hides the entire bubble UI when done
    public void Hide() => gameObject.SetActive(false);
}
