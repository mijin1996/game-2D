using System;
using UnityEngine;
using AppsInToss;

public class AM : MonoBehaviour {
    private static AM am;

    public static AM ins {
        get { return am; }
    }

    [Header("광고 그룹 ID (개발 중에는 테스트 ID)")]
    public string bannerId = "ait-ad-test-banner-id";
    public string interstitialId = "ait-ad-test-interstitial-id";
    // public string rewardedId = "ait-ad-test-rewarded-id";

    [Header("전면 광고")]
    public int showEveryDeaths = 3;

    private static int deathCnt = 0;
    private bool interstitialReady = false;
    private Action interstitialLoadUnsubscribe;
    private Action interstitialShowUnsubscribe;

    // private bool rewardedReady = false;
    // private Action rewardedLoadUnsubscribe;
    // private Action rewardedShowUnsubscribe;

    private void Awake() {
        if (am == null) {
            am = this;
        } else if (am != this) {
            // 두개 이상의 광고 매니저가 존재하지 않도록 위함
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        LoadInterstitial();
        // LoadRewarded();
    }

    // 배너 광고 표시 (화면 하단)
    public void ShowBanner() {
        AITBannerAd.Show(bannerId, AITBannerPosition.Bottom);
    }

    // 배너 광고 숨기기
    public void HideBanner() {
        AITBannerAd.Hide();
    }

    // 전면 광고 미리 불러오기 (loaded 이벤트가 오면 보여줄 준비 완료)
    private void LoadInterstitial() {
        interstitialReady = false;
        interstitialLoadUnsubscribe?.Invoke();
        interstitialLoadUnsubscribe = AIT.LoadFullScreenAd(
            adGroupId: interstitialId,
            onEvent: adEvent => {
                if (adEvent.Type == "loaded") { interstitialReady = true; }
            },
            onError: err => Debug.LogWarning("전면 광고 로드 실패: " + err.Message)
        );
    }

    // 게임오버 때 호출 : showEveryDeaths판마다 전면 광고 표시
    public void OnGameOver() {
        deathCnt++;
        if (!interstitialReady || deathCnt % showEveryDeaths != 0) { return; }

        interstitialReady = false;
        interstitialShowUnsubscribe?.Invoke();
        interstitialShowUnsubscribe = AIT.ShowFullScreenAd(
            adGroupId: interstitialId,
            onEvent: adEvent => {
                if (adEvent.Type == "dismissed") { LoadInterstitial(); }
            },
            onError: err => Debug.LogWarning("전면 광고 표시 실패: " + err.Message)
        );
    }

    // ===== 보상형 광고 (나중에 사용할 때 주석 해제) =====
    // 사용 예 : AM.ins.ShowRewarded(() => { /* 부활, 점수 2배 등 보상 */ });
    // 보상은 userEarnedReward 이벤트에서만 지급 (dismissed에서 지급하면 정책 위반)
    //
    // void LoadRewarded() {
    //     rewardedReady = false;
    //     rewardedLoadUnsubscribe?.Invoke();
    //     rewardedLoadUnsubscribe = AIT.LoadFullScreenAd(
    //         adGroupId: rewardedId,
    //         onEvent: adEvent => {
    //             if (adEvent.Type == "loaded") { rewardedReady = true; }
    //         },
    //         onError: err => Debug.LogWarning("보상형 광고 로드 실패: " + err.Message)
    //     );
    // }
    //
    // public bool IsRewardedReady() {
    //     return rewardedReady;
    // }
    //
    // public void ShowRewarded(Action onReward) {
    //     if (!rewardedReady) { return; }
    //
    //     rewardedReady = false;
    //     rewardedShowUnsubscribe?.Invoke();
    //     rewardedShowUnsubscribe = AIT.ShowFullScreenAd(
    //         adGroupId: rewardedId,
    //         onEvent: adEvent => {
    //             if (adEvent.Type == "userEarnedReward") { onReward?.Invoke(); }
    //             if (adEvent.Type == "dismissed") { LoadRewarded(); }
    //         },
    //         onError: err => Debug.LogWarning("보상형 광고 표시 실패: " + err.Message)
    //     );
    // }

    private void OnDestroy() {
        interstitialLoadUnsubscribe?.Invoke();
        interstitialShowUnsubscribe?.Invoke();
        // rewardedLoadUnsubscribe?.Invoke();
        // rewardedShowUnsubscribe?.Invoke();
    }
}
