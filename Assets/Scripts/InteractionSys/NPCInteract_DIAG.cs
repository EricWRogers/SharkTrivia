using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class NPCInteract_DIAG : MonoBehaviour
{
    [Header("Dialogue")]
    public Conversation conversation;
    public DNode optionalStartNode;
    public KeyCode interactKey = KeyCode.E;

    [Header("Prompt UI (optional)")]
    public CanvasGroup promptGroup;        // assign the panel with CanvasGroup
    public TMP_Text promptText;
    [TextArea] public string promptMessage = "Press E to talk";

    [Header("Proximity (works even if triggers fail)")]
    public bool useProximity = true;
    public float radius = 2.0f;
    public LayerMask playerMask;           // include Player layer
    public string playerTag = "Player";

    // --- runtime ---
    bool _playerInsideTrigger = false;
    Transform _player;
    float _nextTick;

    void Awake()
    {
        Debug.Log($"[NPCInteract] Awake on {name}");
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;     // ensure trigger
    }

    void OnEnable()
    {
        Debug.Log($"[NPCInteract] OnEnable on {name}");
        HidePromptImmediate();
    }

    void Update()
    {
        // heartbeat (1/sec)
        if (Time.time >= _nextTick)
        {
            _nextTick = Time.time + 1f;
            bool panelOpenDbg = DialogueController.Instance &&
                                DialogueController.Instance.dialoguePanel &&
                                DialogueController.Instance.dialoguePanel.activeInHierarchy;
            Debug.Log($"[NPCInteract] tick inRange={_playerInsideTrigger || (useProximity && ProximityCheck())} panelOpen={panelOpenDbg}");
        }

        bool panelOpen = DialogueController.Instance &&
                        DialogueController.Instance.dialoguePanel &&
                        DialogueController.Instance.dialoguePanel.activeInHierarchy;

        // compute range (triggers OR proximity)
        bool inRange = _playerInsideTrigger || (useProximity && ProximityCheck());

        // show/hide prompt – show only when panel is closed AND player is in range
        SetPrompt(inRange && !panelOpen);

        // read E only when panel is closed and in range
        if (!panelOpen && inRange && Input.GetKeyDown(interactKey))
        {
            Debug.Log("[NPCInteract] E pressed while in range");
            StartDialogue();
        }
    }

    bool ProximityCheck()
    {
        if (_player == null)
        {
            var go = GameObject.FindGameObjectWithTag(playerTag);
            if (go) _player = go.transform;
        }
        if (_player == null) return false;

        float dist = Vector2.Distance(_player.position, transform.position);
        return dist <= radius;
    }

    void StartDialogue()
    {
        if (!conversation) { Debug.LogWarning("[NPCInteract] No Conversation assigned."); return; }
        var dm = DialogueManagerIntegrated.Instance;
        if (!dm) { Debug.LogError("[NPCInteract] DialogueManagerIntegrated.Instance is null"); return; }

        HidePromptImmediate();
        if (optionalStartNode)
        {
            Debug.Log("[NPCInteract] StartConversationAt (specific node)");
            dm.StartConversationAt(conversation, optionalStartNode);
        }
        else
        {
            Debug.Log("[NPCInteract] StartConversation (entry)");
            dm.StartConversation(conversation);
        }
    }

    // --- Trigger path (optional) ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        _playerInsideTrigger = true;
        _player = other.transform;
        Debug.Log("[NPCInteract] OnTriggerEnter2D -> in range");
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        _playerInsideTrigger = false;
        if (_player == other.transform) _player = null;
        Debug.Log("[NPCInteract] OnTriggerExit2D -> out of range");
    }

    // --- Prompt helpers ---
    void SetPrompt(bool visible)
    {
        if (!promptGroup) return;
        if (visible)
        {
            if (!promptGroup.gameObject.activeSelf) promptGroup.gameObject.SetActive(true);
            promptGroup.alpha = 1f;
            promptGroup.interactable = false;
            promptGroup.blocksRaycasts = false;
            if (promptText) promptText.text = promptMessage;
        }
        else
        {
            promptGroup.alpha = 0f;
            promptGroup.interactable = false;
            promptGroup.blocksRaycasts = false;
        }
    }
    void HidePromptImmediate()
    {
        if (!promptGroup) return;
        promptGroup.alpha = 0f;
        promptGroup.interactable = false;
        promptGroup.blocksRaycasts = false;
        // keep GO active; we only hide by alpha so parenting under a disabled object won’t break us
        if (!promptGroup.gameObject.activeSelf) promptGroup.gameObject.SetActive(true);
    }

    // visualize proximity
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
