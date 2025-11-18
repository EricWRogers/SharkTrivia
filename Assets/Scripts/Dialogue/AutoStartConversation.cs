using UnityEngine;

public class AutoStartConversation : MonoBehaviour
{
    public Conversation conversation;

    void Start()
    {
        Debug.Log("starting conversation: " + conversation);
        DialogueManagerIntegrated.Instance.StartConversation(conversation);
    }
}

