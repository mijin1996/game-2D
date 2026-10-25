using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour {
    public GameObject platformSnailPrefab;
    public GameObject platformCoinPrefab;

    public int count = 3;

    [Header("규칙적인 랜덤 배치")]
    public int snailPerRound = 1;
    public int coinPerRound = 1;

    public float timeBetSpawnMin = 1.25f;
    public float timeBetSpawnMax = 2.25f;
    private float timeBetSpawn;

    public float yMin = -3.5f;
    public float yMax = 1.5f;
    private float xPos = 20f;

    private GameObject[] snailPlatforms;
    private GameObject[] coinPlatforms;
    private int snailIndex = 0;
    private int coinIndex = 0;

    private List<bool> bag = new List<bool>();

    private Vector2 pool = new Vector2(0, -25);
    private float lastSpawnTime;

    private void Start() {

        // 변수들을 초기화하고 사용할 발판들을 미리 생성
        // 예전에는 시작할 때 발판 3개의 종류를 한 번만 랜덤으로 정해서
        // (예: 코인 3개가 뽑히면) 게임 내내 달팽이가 안 나오는 문제가 있었다.
        // 이제는 종류별로 따로 만들어 두고, 배치할 때마다 종류를 고른다.
        snailPlatforms = new GameObject[count];
        coinPlatforms = new GameObject[count];

        for (int i = 0; i < count; i++) {
            snailPlatforms[i] = Instantiate(platformSnailPrefab, pool, Quaternion.identity);
            coinPlatforms[i] = Instantiate(platformCoinPrefab, pool, Quaternion.identity);
        }

        lastSpawnTime = 0f; //마지막으로 발판 생성 시간
        timeBetSpawn = 0f; //다음 발판 생성까지의 시간 간격
    }

    private void Update() {
        // 순서를 돌아가며 주기적으로 발판을 배치
        if (GM.ins.isGameover) {
            return;
        }

        if (Time.time >= lastSpawnTime + timeBetSpawn) {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            float yPos = Random.Range(yMin, yMax);

            GameObject platform;
            if (NextIsSnail()) {
                platform = snailPlatforms[snailIndex];
                snailIndex = (snailIndex + 1) % count;
            } else {
                platform = coinPlatforms[coinIndex];
                coinIndex = (coinIndex + 1) % count;
            }

            platform.SetActive(false);
            platform.transform.position = new Vector2(xPos, yPos);

            Coin[] coins = platform.GetComponentsInChildren<Coin>(true); // GetComponentsInChildren<T>(true) => 플랫폼의 자식(Coin)의 활성화 여부와 상관없이 모든 컴포넌트를 가져옴 📌🪙
            for (int i = 0; i < coins.Length; i++) {
                coins[i].gameObject.SetActive(true);
            }
            platform.SetActive(true);
        }
    }

    private bool NextIsSnail() {
        if (bag.Count == 0) {
            for (int i = 0; i < snailPerRound; i++) bag.Add(true);
            for (int i = 0; i < coinPerRound; i++) bag.Add(false);
            if (bag.Count == 0) { bag.Add(true); bag.Add(false); }

            // 순서 섞기 (Fisher-Yates)
            for (int i = bag.Count - 1; i > 0; i--) {
                int j = Random.Range(0, i + 1);
                bool tmp = bag[i]; bag[i] = bag[j]; bag[j] = tmp;
            }
        }

        bool result = bag[0];
        bag.RemoveAt(0);
        return result;
    }
}

/*
// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public int count = 3; // 생성할 발판의 개수

    public float timeBetSpawnMin = 1.25f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.25f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격

    public float yMin = -3.5f; // 블럭 배치할 위치의 최소 y값
    public float yMax = 1.5f; // 블럭 배치할 위치의 최대 y값
    private float xPos = 20f; // 블럭 배치할 위치의 x 값

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 pool = new Vector2(0, -25); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막 배치 시점

    void Start() {
        // 변수들을 초기화하고 사용할 발판들을 미리 생성
        platforms = new GameObject[count]; // 실제 메모리 할당된 것을 의미 ||  *역할을 한다
        for(int i=0 ; i<count; i++) {
            platforms[i] = Instantiate(platformPrefab, pool, Quaternion.identity); // 할당은 Instantiate로 이루어짐 || 📌Quaternion.identity회전값이 없는 초기화
        }
        lastSpawnTime = 0f; // 마지막 배치 시점 초기화
        timeBetSpawn=0f; // 다음 배치까지의 시간 간격 초기화
    }

    void Update() {
        // 순서를 돌아가며 주기적으로 발판을 배치
    }
}
*/
