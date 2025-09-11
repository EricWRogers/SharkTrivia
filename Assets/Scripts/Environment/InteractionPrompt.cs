using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    public static InteractionPrompt Instance { get; private set; }
    public static bool HasInstance => Instance != null;

    public RectTransform root;
    public TextMeshProUGUI label;
    public Vector2 screenOffset = new Vector2(0, 40);

    Camera cam;

    // current state to show dialogue
    bool visible;
    string cachedText;
    Vector3 cachedWorldPos;

    void Awake()
    {
        Instance = this;
        cam = Camera.main;
        if (root == null) root = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void SetVisible(bool show, string text, Vector3 worldPos)
    {
        visible = show;
        cachedText = text;
        cachedWorldPos = worldPos;

        if (!visible)
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
            return;
        }

        if (!gameObject.activeSelf) gameObject.SetActive(true);
        if (label) label.text = $"E - {cachedText}";
    }

    void LateUpdate()
    {
        if (!visible) return;
        if (!cam) cam = Camera.main;

        Vector3 screen = RectTransformUtility.WorldToScreenPoint(cam, cachedWorldPos) + screenOffset;
        root.position = screen;
    }
}
