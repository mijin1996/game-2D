using UnityEngine;
using BlackMassSoftware.FloatingTextEngine.Lite;
using BlackMassSoftware.FloatingTextEngine.Lite.Behaviors;

public class FloatEffect : MonoBehaviour, IEffect {
    [Header("Floating Text")]
    public Vector3 textOffset = new Vector3(0f, 0.8f, 0f);
    public Color textColor = Color.yellow;

    [Header("Effect")]
    public float popScale = 1.4f;
    public float moveUpDistance = 1.2f;
    public float effectDuration = 0.6f;

    // IEffect: 숫자 없이 부르면 "+1"
    public void Play() {
        Play(1);
    }

    public void Play(int scoreAmount) {
        Vector3 spawnPosition = transform.position + textOffset; // 텍스트가 생성될 위치를 계산 (현재 오브젝트의 위치 + 오프셋)

        FloatingTextEngine
            .CreateTextAtWorldSpace(spawnPosition, "+" + scoreAmount, textColor)
            .With(FloatingTextBehaviors.Pop(popScale, 0.08f, 0.15f))
            .With(FloatingTextBehaviors.MoveUp(moveUpDistance, effectDuration))
            .With(FloatingTextBehaviors.FadeOut(effectDuration));
    }
}
