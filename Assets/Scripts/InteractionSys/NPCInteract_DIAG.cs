using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class NPCInteract_DIAG : MonoBehaviour
{
    [Header("Dialogue")]
    public Conversation conversation;
    public DNode optionalStartNode;
    public KeyCode interactKey = KeyCode.E;

    [Header("Prompt UI (optional)")]
    public CanvasGroup promptGroup;   // Assign a small “Press E” panel in Canvas
    public TMP_Text promptText;       // The TMP label inside it
    [SerializeField] string promptMessage = "Press E to talk";

    [Header("Fallback Proximity (if trigger fails)")]
    public bool useProximityFallback = true;
    public float fallbackRadius = 1.8f;
    public LayerMask playerMask;  // set to Player’s layer (or Everything)

    bool playerInTrigger = false;
    Transform player;

    void Awake()
    {
        var col = GetComponent<Collider2D>();
        if (col == null) Debug.LogError("[NPCInteract] No Collider2D on NPC.");
        else if (!col.isTrigger)
        {
            Debug.LogWarning("[NPCInteract] Collider2D is not set as Trigger. Setting it now.");
            col.isTrigger = true;
        }
    }

    void Start()
    {
        // Find player
        var p = GameObject.FindGameObjectWithTag("Player");
        if (!p)
        {
            Debug.LogError("[NPCInteract] No object tagged 'Player' found in scene.");
        }
        else
        {
            player = p.transform;
            var rb2d = p.GetComponent<Rigidbody2D>();
            if (!rb2d) Debug.LogError("[NPCInteract] Player has no Rigidbody2D (required for 2D triggers).");
            else if (!rb2d.simulated) Debug.LogError("[NPCInteract] Player Rigidbody2D is not simulated.");
        }

        // Validate Dialogue singletons
        if (!DialogueController.Instance)
            Debug.LogError("[NPCInteract] DialogueController.Instance is null (is there exactly one DialogueController in scene and Awake ran?).");
        if (!DialogueManagerIntegrated.Instance)
            Debug.LogError("[NPCInteract] DialogueManagerIntegrated.Instance is null (is the manager active in scene?).");

        if (promptText) promptText.text = promptMessage;
        SetPromptVisible(false);
    }

    void Update()
    {
        bool inRange = playerInTrigger;

        // Fallback if triggers aren’t firing
        if (!inRange && useProximityFallback && player)
        {
            var hit = Physics2D.OverlapCircle(transform.position, fallbackRadius, playerMask.value == 0 ? ~0 : playerMask);
            inRange = hit && hit.CompareTag("Player");
        }

        if (!inRange)
        {
            SetPromptVisible(false);
            return;
        }

        // Don’t show prompt during dialogue
        if (IsDialogueOpen())
        {
            SetPromptVisible(false);
            return;
        }

        SetPromptVisible(true);

        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("[NPCInteract] E pressed. Starting conversation…");
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        var dm = DialogueManagerIntegrated.Instance;
        var dc = DialogueController.Instance;
        if (!dm || !dc) { Debug.LogError("[NPCInteract] Dialogue singletons missing."); return; }
        if (!conversation) { Debug.LogError("[NPCInteract] Conversation asset not assigned on NPC."); return; }

        if (optionalStartNode)
        {
            Debug.Log("[NPCInteract] StartConversationAt(optional node).");
            dm.StartConversationAt(conversation, optionalStartNode);
        }
        else
        {
            Debug.Log("[NPCInteract] StartConversation(entry).");
            dm.StartConversation(conversation);
        }
    }

    bool IsDialogueOpen()
    {
        var dc = DialogueController.Instance;
        return dc && dc.dialoguePanel && dc.dialoguePanel.activeSelf;
    }

    void SetPromptVisible(bool show)
    {
        if (!promptGroup) return;
        promptGroup.alpha = show ? 1f : 0f;
        promptGroup.blocksRaycasts = show;
        promptGroup.interactable = show;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            Debug.Log("[NPCInteract] OnTriggerEnter2D: Player entered.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            Debug.Log("[NPCInteract] OnTriggerExit2D: Player exited.");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (useProximityFallback)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, fallbackRadius);
        }
    }
}
