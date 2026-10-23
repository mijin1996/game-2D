using UnityEngine;

public class PlatformSpawner : MonoBehaviour {
    public GameObject prefab;
    public int count = 3;

    public float timeBetSpawnMin = 1.25f;
    public float timeBetSpawnMax = 2.25f;
    private float timeBetSpawn;

    public float yMin = -3.5f;
    public float yMax = 1.5f;
    private float xPos = 20f;

    private GameObject[] platforms;
    private int nextPlatformIndex = 0;

    private Vector2 pool = new Vector2(0, -25); // 플랫폼의 대기소 위치
    private float lastSpawnTime;

    void Start() {
        platforms = new GameObject[count];

        for (int i = 0; i < count; i++) {
            platforms[i] = Instantiate(prefab, pool, Quaternion.identity);
        }

        lastSpawnTime = 0f;
        timeBetSpawn = 0f;
    }

    void Update() {
        if (GM.gm.isGameover) {
            return;
        }

        if (Time.time >= lastSpawnTime + timeBetSpawn) {
            lastSpawnTime = Time.time;

            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            float yPos = Random.Range(yMin, yMax); // 대기 위치(아래 -25)에 있던 발판을 스폰 위치(오른쪽 x=20)로 옮기면서, 높이는 매번 랜덤하게 정함

            platforms[nextPlatformIndex].SetActive(false);
            platforms[nextPlatformIndex].SetActive(true);

            platforms[nextPlatformIndex].transform.position = new Vector2(xPos, yPos);
            nextPlatformIndex++;

            if (nextPlatformIndex >= count) {
                nextPlatformIndex = 0;
            }
        }
    }
}