using System;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    public void PlaySFX(String _name)
    {
        AudioManager.instance.PlaySFX(_name);
    }
}
