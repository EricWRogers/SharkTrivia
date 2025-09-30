using UnityEngine;

public class AutoStartConversation : MonoBehaviour
{
    public Conversation conversation;
    

    void Start()
    {
        DialogueManagerIntegrated.Instance.StartConversation(conversation);
    }
}

