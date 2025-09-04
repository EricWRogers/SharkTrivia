using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    public static InteractionPrompt Instance { get; private set; }
    public static bool HasInstance => Instance != null;

    public RectTransform root;
    public TextMeshProUGUI label;
    public Vector3 screenOffset = new Vector3(0, 40, 0);

    Camera cam;

    void Awake()
    {
        Instance = this;
        cam = Camera.main;
        if (root == null) root = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void SetVisible(bool visible, string text, Vector3 worldPos)
    {
        if (!visible) { gameObject.SetActive(false); return; }
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        if (label) label.text = text.Length > 0 ? $"E - {text}" : "E - Interact";

        Vector3 screen = cam.WorldToScreenPoint(worldPos) + screenOffset;
        root.position = screen;
    }
}
