using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuIntro : MonoBehaviour
{
    [Header("Refs")]

    public RectTransform title;
    public CanvasGroup titleCanvasGroup;
    public List<RectTransform> bubbleButtons = new List<RectTransform>();

    [Header("Slide In")]
    [Tooltip("How far off-screen to start")]
    public float verticalOffset = 300f;
    public float titleSlideDuration = 0.65f;
    public float buttonSlideDuration = 0.55f;
    public float buttonStagger = 0.08f;   // delay between each button’s slide

    [Header("Float Loop")]
    public float floatAmplitude = 8f;      // pixels
    public float floatSpeed = 1.0f;        // cycles per second
    public float floatPhaseOffset = 0.4f;  // make them float offset

    [Header("Flash (title)")]
    [Tooltip("How fast the title flashes.")]
    public float flashSpeed = 1.2f; 
    public float flashMin = 0.85f;
    public float flashMax = 1.0f;

    [Header("Input")]
    public bool disableButtonsUntilShown = true;

    private Vector2 _titleTargetPos;
    private Vector2[] _buttonTargetPos;
    private List<Button> _buttons = new List<Button>();
    private bool _started;

    void Awake()
    {
        if (!title) Debug.LogWarning("[MainMenuIntro] Title is not assigned.");
        if (title && !titleCanvasGroup)
        {
            titleCanvasGroup = title.GetComponent<CanvasGroup>();
            if (!titleCanvasGroup) titleCanvasGroup = title.gameObject.AddComponent<CanvasGroup>();
        }

       
        if (title)
        {
            _titleTargetPos = title.anchoredPosition;
            title.anchoredPosition = _titleTargetPos + new Vector2(0f, verticalOffset); // start above
            titleCanvasGroup.alpha = 0f;
        }

        _buttonTargetPos = new Vector2[bubbleButtons.Count];
        _buttons.Clear();

        for (int i = 0; i < bubbleButtons.Count; i++)
        {
            var rt = bubbleButtons[i];
            if (!rt) continue;

            _buttonTargetPos[i] = rt.anchoredPosition;
            rt.anchoredPosition = _buttonTargetPos[i] + new Vector2(0f, -verticalOffset); // start below

            // Gather Button on the same object
            var btn = rt.GetComponentInChildren<Button>(true);
            if (btn)
            {
                _buttons.Add(btn);
                if (disableButtonsUntilShown) btn.interactable = false;
            }
        }
    }

    void OnEnable()
    {
        // only run once per activation
        if (_started) return;
        _started = true;

        StartCoroutine(RunIntroSequence());
    }

    private IEnumerator RunIntroSequence()
    {
        
        if (title)
        {
            yield return SlideRect(title, title.anchoredPosition, _titleTargetPos, titleSlideDuration, titleCanvasGroup);
            // Start flashing once it lands
            StartCoroutine(FlashTitle());
        }

        // 2) Slide in Buttons
        for (int i = 0; i < bubbleButtons.Count; i++)
        {
            var rt = bubbleButtons[i];
            if (!rt) continue;

            // slide this button
            StartCoroutine(SlideRect(rt, rt.anchoredPosition, _buttonTargetPos[i], buttonSlideDuration, null, i, onComplete: () =>
            {
                // Begin floating
                StartCoroutine(FloatLoop(rt, _buttonTargetPos[i], i));

                // Enable its button
                var btn = rt.GetComponentInChildren<Button>(true);
                if (btn && disableButtonsUntilShown) btn.interactable = true;
            }));

            yield return new WaitForSeconds(buttonStagger);
        }
    }

    private IEnumerator SlideRect(RectTransform rt, Vector2 from, Vector2 to, float duration, CanvasGroup cg = null, int index = 0, System.Action onComplete = null)
    {
        float t = 0f;

       
        float startAlpha = cg ? cg.alpha : 1f;
        float endAlpha = cg ? 1f : startAlpha;

        while (t < duration)
        {
            float p = t / duration;
           
            float e = 1f - Mathf.Pow(1f - p, 3f);

            rt.anchoredPosition = Vector2.LerpUnclamped(from, to, e);

            if (cg) cg.alpha = Mathf.Lerp(startAlpha, endAlpha, e);

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        rt.anchoredPosition = to;
        if (cg) cg.alpha = endAlpha;

        onComplete?.Invoke();
    }

    private IEnumerator FloatLoop(RectTransform rt, Vector2 basePos, int index)
    {
        float phase = index * floatPhaseOffset; // offset each one
        while (true)
        {
            float y = Mathf.Sin((Time.unscaledTime + phase) * (Mathf.PI * 2f) * floatSpeed) * floatAmplitude;
            rt.anchoredPosition = basePos + new Vector2(0f, y);
            yield return null;
        }
    }

    private IEnumerator FlashTitle()
    {
        
        while (true)
        {
            float s = (Mathf.Sin(Time.unscaledTime * Mathf.PI * 2f * flashSpeed) + 1f) * 0.5f; 
            float a = Mathf.Lerp(flashMin, flashMax, s);
            if (titleCanvasGroup) titleCanvasGroup.alpha = a;
            yield return null;
        }
    }
}

