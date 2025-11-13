using UnityEngine;

public class AutoStartConversation : MonoBehaviour
{
    public Conversation conversation;
    public bool cipherEnabled = true;

    void Start()
    {
        TempCipherEncoder.Enabled = cipherEnabled; // toggle cipher for this scene
        Debug.Log("starting conversation: " + conversation);
        DialogueManagerIntegrated.Instance.StartConversation(conversation);
    }
}

