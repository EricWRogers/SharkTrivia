using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class NPCInteract : MonoBehaviour
{
    [Header("Dialogue")]
    public Conversation conversation;         
    public DNode optionalStartNode;           
    public KeyCode interactKey = KeyCode.E;

    [Header("Prompt UI (optional)")]
    public CanvasGroup promptGroup;          
    public TMP_Text promptText;              
    [SerializeField] private string promptMessage = "Press E to talk";

    private bool _playerInRange;

    void Reset()
    {
        
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void Start()
    {
        SetPromptVisible(false);
        if (promptText) promptText.text = promptMessage;
    }

    void Update()
    {
        
        if (!_playerInRange) return;
        if (IsDialogueOpen()) return;

        
        if (Input.GetKeyDown(interactKey))
        {
            // Hide prompt while talking
            SetPromptVisible(false);

            if (optionalStartNode)
                DialogueManagerIntegrated.Instance.StartConversationAt(conversation, optionalStartNode);
            else
                DialogueManagerIntegrated.Instance.StartConversation(conversation);
        }
    }

    private bool IsDialogueOpen()
    {
        var ui = DialogueController.Instance;
        return ui != null && ui.dialoguePanel != null && ui.dialoguePanel.activeSelf;
    }

    private void SetPromptVisible(bool show)
    {
        if (!promptGroup) return;
        promptGroup.alpha = show ? 1f : 0f;
        promptGroup.blocksRaycasts = show;
        promptGroup.interactable = show;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = true;

        // Only show prompt if dialogue isn’t already open
        if (!IsDialogueOpen()) SetPromptVisible(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = false;
        SetPromptVisible(false);
    }
}
