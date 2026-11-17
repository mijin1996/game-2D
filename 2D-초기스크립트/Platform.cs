using UnityEngine;

public class Platform : MonoBehaviour {
    public GameObject[] obstacles; // 장애물
    private bool stepped = false;

    private void OnEnable() {
        stepped = false;

        for (int i = 0; i < obstacles.Length; i++) {
            if (Random.Range(0, 3) == 0) {
                obstacles[i].SetActive(true);
            } else {
                obstacles[i].SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Player" && !stepped) { // "": string을 의미, 에디터에서 붙인 이름을 찾을때 || text || log에 사용
            stepped = true;
            GM.gm.AddScore(1);
        }
    }
}