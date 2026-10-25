using UnityEngine;

public class Platform : MonoBehaviour {
    public GameObject[] obstacles;
    [Range(0f, 1f)] public float obstacleChance = 1f;
    private bool stepped = false;

    private void OnEnable() {
        // 발판을 리셋하는 처리
        stepped = false;

        for (int i = 0; i < obstacles.Length; i++) {
            if (obstacles[i] == null) { continue; }
            obstacles[i].SetActive(Random.value < obstacleChance);
        }
    }
}
