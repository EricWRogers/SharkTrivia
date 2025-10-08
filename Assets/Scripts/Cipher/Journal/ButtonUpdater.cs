using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor.Animations;
using JetBrains.Annotations;
using System.Collections;
using TMPro;

public class ButtonUpdater : MonoBehaviour
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
        buttonText.text = " B";
    }

}