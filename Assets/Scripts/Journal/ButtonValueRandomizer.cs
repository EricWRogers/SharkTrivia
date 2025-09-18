using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonValueRandomizer : MonoBehaviour
{
    
    public TMP_Text buttonText;
    
    void Start()
    {

    }
    
    
    void Update()
    {

    }

    public void NewText()
    {
        buttonText.text += " B";
    }

}
