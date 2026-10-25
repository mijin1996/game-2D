using UnityEngine;
using UnityEngine.UI;

public class BlinkEffect : MonoBehaviour, IEffect {
    [Header("깜빡임")]
    public Color blinkColor = Color.red;
    public int blinkCount = 4;
    public float blinkInterval = 0.12f;
    public bool stayRedAfterBlink = false;

    private Graphic target;
    private Color originalColor;
    private float blinkTimer = -1f;

    private void Awake() {
        target = GetComponent<Graphic>();
        if (target != null) { originalColor = target.color; }
    }

    // 깜빡임 시작
    public void Play() {
        if (target == null) { return; }
        blinkTimer = 0f;
        target.color = blinkColor;
    }

    private void Update() {
        if (blinkTimer < 0f || target == null) { return; }

        blinkTimer += Time.unscaledDeltaTime;
        int step = (int)(blinkTimer / blinkInterval); // 0,1,2,3... 짝수=blinkColor, 홀수=원래색

        if (step >= blinkCount * 2) {
            target.color = stayRedAfterBlink ? blinkColor : originalColor;
            blinkTimer = -1f;
            return;
        }

        target.color = (step % 2 == 0) ? blinkColor : originalColor;
    }
}
