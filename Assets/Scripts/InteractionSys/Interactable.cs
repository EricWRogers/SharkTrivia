using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string displayName = "Interact"; //this will change who you're interacting with
    public KeyCode interactKey = KeyCode.E; //the key to press to begin the interaction
    public float interactRange = 2.0f; //range that you can press E to begin interaction
    public Transform worldPromptAnchor; //where the popup for the interaction is

    public UnityEvent onInteract; 

    Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        if (!worldPromptAnchor) worldPromptAnchor = transform;
    }

    void Update()
    {
        if (!player) return;
        float dist = Vector2.Distance(player.position, transform.position);
        bool inRange = dist <= interactRange;

        InteractionPrompt.Instance?.SetVisible(inRange, displayName, worldPromptAnchor.position);

        if (inRange && Input.GetKeyDown(interactKey))
            onInteract?.Invoke();
    }

    void OnDisable()
    {
        if (InteractionPrompt.HasInstance)
            InteractionPrompt.Instance.SetVisible(false, "", Vector3.zero);
    }
}
