using UnityEngine;

public class AutoStartConversation : MonoBehaviour
{
    public Conversation conversation;
    public bool cipherEnabled = true;

    void Start()
    {
        TempCipherEncoder.Enabled = cipherEnabled; // toggle cipher for this scene
        DialogueManagerIntegrated.Instance.StartConversation(conversation);
    }
}

