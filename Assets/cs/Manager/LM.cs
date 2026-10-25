using UnityEngine;
using TMPro;

public class LM : MonoBehaviour {
    private static LM lm;

    public static LM ins {
        get { return lm; }
    }

    public AudioClip levelUpClip;
    [Range(0f, 1f)] public float levelUpVolume = 0.53f;
    public GameObject levelUpEffectPrefab;
    public float levelUpEffectLifeTime = 0.5f;
    public bool effectAtLevelText = true;

    public TextMeshProUGUI levelText;

    public int level = 1;
    public int coinsForLevelUp = 5;

    private int currentCoinCnt = 0;

    private IEffect levelBlinkEffect;

    private void Awake() {
        if (lm == null) {
            lm = this;
        } else if (lm != this) {
            // 두개 이상의 레벨 매니저가 존재하지 않도록 위함
            Destroy(gameObject);
            return;
        }

        if (levelText != null) {
            // 레벨 글자에 BlinkEffect가 없으면 자동으로 붙인다 (설정값은 레벨 글자의 BlinkEffect에서 조절)
            BlinkEffect blink = levelText.GetComponent<BlinkEffect>();
            if (blink == null) { blink = levelText.gameObject.AddComponent<BlinkEffect>(); }
            levelBlinkEffect = blink;
        }
    }

    // UI(레벨 글자)의 화면 위치를 게임 월드 좌표로 바꿔서 이펙트를 글자 위치에 띄운다
    private Vector3 GetEffectPosition() {
        if (!effectAtLevelText || levelText == null || Camera.main == null) {
            return transform.position;
        }

        Canvas canvas = levelText.canvas;
        Camera uiCamera = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null;
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, levelText.rectTransform.position);

        Camera cam = Camera.main;
        float depth = -cam.transform.position.z; // 2D 게임: z=0 평면에 생성
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, depth));
        worldPos.z = 0f;
        return worldPos;
    }

    public void AddCoin(int amount) {  // 코인을 추가하고 레벨업 여부를 확인하는 메서드
        currentCoinCnt += amount;

        if (currentCoinCnt >= coinsForLevelUp) {
            LevelUp();
        }
    }

    private void LevelUp() {
        level++;
        currentCoinCnt = 0;

        if (levelText != null) {
            levelText.text = "LV. " + level;
            levelBlinkEffect.Play(); // 레벨 글자 빨간색 깜빡임
            SM.ins.LevelUpSound(levelUpClip, levelUpVolume); // 레벨업 전용 스피커 (원래처럼 다시 울리면 처음부터)

            if (levelUpEffectPrefab != null) {
                GameObject destryEffect = Instantiate(levelUpEffectPrefab, GetEffectPosition(), Quaternion.identity);// 레벨업 이펙트 생성
                Destroy(destryEffect, levelUpEffectLifeTime);
            }
        }

        Debug.Log("레벨업! 현재 레벨: " + level);

    }

}
