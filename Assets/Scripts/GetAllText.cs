using TMPro;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Turns out that even if you change the default font in unity, text made already WONT update to the new font :))
/// So this grabs all text with the original default font and updates it to Pangolin, to save time and prevent accidentally missing one
/// </summary>
public class GetAllText : MonoBehaviour
{
    public TMP_FontAsset newDesiredFont;
    public TextMeshProUGUI[] allTexts;

    void OnValidate()
    {
        //find all text mesh pro text in scene
        allTexts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);

        foreach (TextMeshProUGUI text in allTexts)
        {
            text.font = newDesiredFont;
        }
    }
}
