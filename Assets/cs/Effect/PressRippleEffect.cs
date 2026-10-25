using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressRippleEffect : MonoBehaviour, IPointerDownHandler, IEffect {
    [Header("눌림 효과")]
    public float pressScale = 0.85f;
    public float punchDuration = 0.15f;

    [Header("퍼지는 빛 고리 효과")]
    public bool useRipple = true;
    public Color rippleColor = new Color(1f, 0.9f, 0.4f, 0.8f);
    public float rippleScale = 1.8f;
    public float rippleDuration = 0.35f;

    private Vector3 originalScale;
    private RectTransform rippleRect;
    private Image rippleImage;

    private float punchTime = -1f;
    private float rippleTime = -1f;

    private void Awake() {
        originalScale = transform.localScale;
        if (useRipple) { CreateRipple(); }
    }

    private void OnDisable() {
        transform.localScale = originalScale;
        punchTime = -1f;
        rippleTime = -1f;
        if (rippleRect != null) { rippleRect.gameObject.SetActive(false); }
    }

    public void OnPointerDown(PointerEventData eventData) {
        Play();
    }

    // 다른 스크립트에서도 호출할 수 있도록 public
    public void Play() {
        if (!isActiveAndEnabled) { return; }

        punchTime = 0f;
        transform.localScale = originalScale * pressScale;

        if (useRipple && rippleRect != null) {
            rippleTime = 0f;
            rippleRect.localScale = originalScale;
            rippleImage.color = rippleColor;
            rippleRect.gameObject.SetActive(true);
        }
    }

    private void Update() {
        float dt = Time.unscaledDeltaTime;

        // 1) 버튼 눌림 → 튀어나옴
        if (punchTime >= 0f) {
            punchTime += dt;
            float k = Mathf.Clamp01(punchTime / punchDuration);
            // 살짝 더 커졌다가 돌아오는 느낌 (EaseOutBack)
            float ease = 1f + 2.70158f * Mathf.Pow(k - 1f, 3f) + 1.70158f * Mathf.Pow(k - 1f, 2f);
            transform.localScale = Vector3.LerpUnclamped(originalScale * pressScale, originalScale, ease);
            if (k >= 1f) {
                transform.localScale = originalScale;
                punchTime = -1f;
            }
        }

        // 2) 빛 고리 퍼짐
        if (rippleTime >= 0f) {
            rippleTime += dt;
            float k = Mathf.Clamp01(rippleTime / rippleDuration);
            rippleRect.localScale = originalScale * Mathf.Lerp(1f, rippleScale, 1f - (1f - k) * (1f - k));
            Color c = rippleColor;
            c.a = rippleColor.a * (1f - k);
            rippleImage.color = c;
            if (k >= 1f) {
                rippleRect.gameObject.SetActive(false);
                rippleTime = -1f;
            }
        }
    }

    // 빛 고리 오브젝트를 한 번만 만들어 둔다
    private void CreateRipple() {
        RectTransform myRect = transform as RectTransform;
        if (myRect == null || transform.parent == null) { return; }

        GameObject ring = new GameObject("AttackRipple", typeof(RectTransform), typeof(Image));
        rippleRect = ring.GetComponent<RectTransform>();
        rippleRect.SetParent(transform.parent, false);
        rippleRect.SetSiblingIndex(transform.GetSiblingIndex()); // 버튼 바로 뒤에 그림

        rippleRect.anchorMin = myRect.anchorMin;
        rippleRect.anchorMax = myRect.anchorMax;
        rippleRect.pivot = myRect.pivot;
        rippleRect.anchoredPosition = myRect.anchoredPosition;
        rippleRect.sizeDelta = myRect.sizeDelta;
        rippleRect.localRotation = myRect.localRotation;

        rippleImage = ring.GetComponent<Image>();
        rippleImage.raycastTarget = false; // 터치를 가로채지 않게
        Image buttonImage = GetComponent<Image>();
        if (buttonImage != null) {
            rippleImage.sprite = buttonImage.sprite;
            rippleImage.type = buttonImage.type;
            rippleImage.preserveAspect = buttonImage.preserveAspect;
        }

        ring.SetActive(false);
    }
}
