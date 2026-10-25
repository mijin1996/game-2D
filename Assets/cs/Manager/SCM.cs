using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SCM : MonoBehaviour {
    public AudioClip changeClip;
    [Range(0f, 1f)] public float changeVolume = 1f;

    [Header("버튼 클릭음 (가이드, 재시작, 홈 버튼 공통)")]
    public AudioClip clickClip;

    [Header("게임 가이드 (기존 ButtonGuide)")]
    public Button guideButton;
    public GameObject guide, closeButton;

    private void Start() {
        // 가이드 버튼이 연결된 씬(Intro)에서만 동작, 다른 씬은 비어 있으므로 건너뜀
        if (guideButton != null) { guideButton.onClick.AddListener(OpenGuide); }
        if (closeButton != null) { closeButton.AddComponent<Button>().onClick.AddListener(CloseGuide); }
    }

    // 가이드 열기 (클릭음 + 가이드 창 켜기)
    public void OpenGuide() {
        SM.ins.Sfx(clickClip);
        guide.SetActive(true);
    }

    // 가이드 닫기 (클릭음 + 가이드 창 끄기)
    public void CloseGuide() {
        SM.ins.Sfx(clickClip);
        guide.SetActive(false);
    }

    public void ChangeScene(int index) {
        StartCoroutine(ChangeSceneRoutine(index)); //StartCoroutine :: “이 함수를 바로 끝까지 실행하지 말고, 중간에 기다리면서 천천히 실행해줘” 라는 뜻입니다.

        //SceneManager.LoadScene(index);

    }

    private IEnumerator ChangeSceneRoutine(int index) {  // 코루틴을 사용하여 씬 전환 전에 소리를 재생하고 일정 시간 대기 후 씬을 전환
        SM.ins.Sfx(changeClip, changeVolume);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(index);
    }

    // 현재 씬 재시작 (클릭음이 끝난 뒤 이동)
    public void RestartScene(int index) {
        StartCoroutine(ClickThenLoadRoutine(index));
    }

    // 홈으로 이동 (클릭음이 끝난 뒤 이동)
    public void HomeButton(int index) {
        StartCoroutine(ClickThenLoadRoutine(index));
    }

    // 버튼 클릭음을 내고, 소리 길이만큼 기다린 뒤 씬 이동 → 씬이 바뀌면서 소리가 잘리지 않음
    private IEnumerator ClickThenLoadRoutine(int index) {
        SM.ins.Sfx(clickClip);
        if (clickClip != null) {
            yield return new WaitForSeconds(clickClip.length);
        }
        SceneManager.LoadScene(index);
    }
}
