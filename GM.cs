using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GM : MonoBehaviour {
    public static GM gm; // instance 할 예정

    public bool isGameover = false;
    public TextMeshProUGUI scoreText;
    public GameObject gameoverUI;

    private int score = 0;

    void Awake() {
        if (gm == null) {
            gm = this;              // 자리가 비어 있으면 → 내가 그 자리에 들어감
        } else {
            Destroy(gameObject);    // 이미 누가 있으면 → 새로 생긴 나를 없앰
        }
    }

    void Update() {
        if (isGameover && Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore(int newScore) {
        if (!isGameover) {
            score += newScore;
            scoreText.text = "Score : " + score;
        }
    }

    public void OnPlayerDead() {
        isGameover = true;
        gameoverUI.SetActive(true);
    }
}