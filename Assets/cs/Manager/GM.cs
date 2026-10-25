using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GM : MonoBehaviour {
    private static GM gm;

    public static GM ins {
        get { return gm; }
    }

    public GameObject CFXR4FallingStars;
    public bool isGameover = false;
    public GameObject gameoverUI;
    public TextMeshProUGUI resultScoreText;

    public TextMeshProUGUI scoreText;
    private int score = 0;

    private IEffect scorePulseEffect;

    [Header("사운드 (SM으로 재생)")]
    public AudioClip introMusicClip;
    public AudioClip mainMusicClip;
    public AudioClip gameOverMusicClip;

    public GameObject endMidtermEffect;
    public AudioClip endMidtermClip;
    public GameObject AttackButton;

    [Range(0f, 1f)]
    public float bgmVolume = 0.5f;
    [Range(0f, 1f)]
    public float gameOverVolume = 1f;

    // 게임 시작과 동시에 싱글톤을 구성
    private void Awake() {
        if (gm == null) {
            gm = this;
        } else if (gm != this) {
            // 두개 이상의 게임 매니저가 존재하지 않도록 위함
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        if (scoreText != null) {
            // 점수 글자에 PulseEffect가 없으면 자동으로 붙인다 (설정값은 점수 글자의 PulseEffect에서 조절)
            PulseEffect pulse = scoreText.GetComponent<PulseEffect>();
            if (pulse == null) { pulse = scoreText.gameObject.AddComponent<PulseEffect>(); }
            scorePulseEffect = pulse;
        }
        PlaySceneMusic();
        GameObject effect = Instantiate(CFXR4FallingStars, Vector3.zero, Quaternion.identity);
    }

    private void PlaySceneMusic() {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0) {  // Intro 씬
            SM.ins.Bgm(introMusicClip, bgmVolume); // BGM 볼륨으로 반복 재생
        } else if (sceneIndex == 1) {  // Main 씬
            SM.ins.Bgm(mainMusicClip, bgmVolume);
        }
    }
    /*
    public void StartButtonSound() {
        SM.ins.Sfx(startButtonClip);
    }
*/
    /* void Update() { if(isGameover && Input.GetMouseButtonDown(0)) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); } // 마우스 || 터치로 재 시작 :: 이 부분을 SCM.cs에서 버튼 처리하도록 함
    } */

    // 점수를 증가시키는 메서드
    public void AddScore(int newScore) {
        if (!isGameover) {
            score += newScore;
            scoreText.text = "Score: " + score; // 점수 UI 갱신
            if (scorePulseEffect != null) { scorePulseEffect.Play(); } // 점수 펄스 시작 (재생 중이면 처음부터 다시)
        }
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead() {
        scoreText.gameObject.SetActive(false);
        LM.ins.levelText.gameObject.SetActive(false);

        resultScoreText.text = "BEST Score: " + score; // 게임오버시 최종 점수를 UI에 표시
        isGameover = true;
        gameoverUI.SetActive(true);
        SM.ins.Sfx(endMidtermClip, bgmVolume); // 기존처럼 BGM 볼륨 크기로 재생
        endMidtermEffect.SetActive(true);
        AttackButton.SetActive(false);
        //endMidtermEffect.SetActive(true);
        //Destroy(endMidtermEffect, 3f);
        //Destroy.Invoke(endMidtermEffect, 2f); // 2초 후에 중간고사 종료 UI 제거 | Invoke는 몇초뒤에 함수 실행하라는 의미📌

        SM.ins.Bgm(gameOverMusicClip, bgmVolume); // 게임오버 음악으로 변경

    }
}

/*using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GM : MonoBehaviour {
    public static GM gm; // 싱글톤을 할당할 전역 변수

    public bool isGameover = false; // 게임 오버 상태
    public TextMeshProUGUI scoreText; // 점수를 출력할 UI 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트

    private int score = 0; // 게임 점수

    // 게임 시작과 동시에 싱글톤을 구성
    void Awake() {
        // 싱글톤 변수 gm가 비어있는가?
        if (gm == null) {
            // gm가 비어있다면(null) 그곳에 자기 자신을 할당
            gm = this;
        } else {
            // gm에 이미 다른 GM 오브젝트가 할당되어 있는 경우

            // 씬에 두개 이상의 GM 오브젝트가 존재한다는 의미.
            // 싱글톤 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    void Update() {
        if(GM.ins.isGameover) {
            return;
        }
        if(Time.time >= lastSpawnTime + timeBetSpawn) {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            float yPos = Random.Range(yMin, yMax);
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);
            currentIndex++;
            if(currentIndex >= count) {
                currentIndex = 0;
            }
        }
        // 게임 오버 상태에서 게임을 재시작할 수 있게 하는 처리
        if (isGameover && Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }

    // 점수를 증가시키는 메서드
    public void AddScore(int newScore) {
        if(!isGameover) {
            score += newScore;
        }
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead() {
        isGameover = true;
        gameoverUI.SetActive(true); //꺼져있는 gameoverUI를 킨다는 의미
    }
}
*/
