using UnityEngine;

public class PulseEffect : MonoBehaviour, IEffect {
    [Header("커졌다 작아졌다")]
    public float pulseScale = 1.3f;
    public int pulseCount = 2;
    public float pulseDuration = 0.4f;

    private Vector3 originalScale = Vector3.one;
    private float pulseTimer = -1f;

    private void Awake() {
        originalScale = transform.localScale;
    }

    // 펄스 시작 (재생 중이면 처음부터 다시)
    public void Play() {
        pulseTimer = 0f;
    }

    private void Update() {
        if (pulseTimer < 0f) { return; }

        pulseTimer += Time.unscaledDeltaTime;
        float k = pulseTimer / pulseDuration;

        if (k >= 1f) {
            transform.localScale = originalScale;
            pulseTimer = -1f;
            return;
        }

        // |sin| 곡선: 0 → 커짐 → 0 을 pulseCount번 반복
        float wave = Mathf.Abs(Mathf.Sin(Mathf.PI * pulseCount * k));
        transform.localScale = originalScale * (1f + (pulseScale - 1f) * wave);
    }
}
