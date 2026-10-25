using UnityEngine;
using TMPro;
using BlackMassSoftware.FloatingTextEngine.Lite;
using BlackMassSoftware.FloatingTextEngine.Lite.Behaviors;

public class Coin : MonoBehaviour {

    private bool stepped = false;
    public GameObject[] coins;
    public AudioClip coinClip;

    public FloatEffect floatEffect;
    private static int coinCnt = 0;

    private void OnEnable() {  //먹었던 코인을 재생성 되도록 하는 부분
        stepped = false;
        for (int i = 0; i < coins.Length; i++) {
            coins[i].SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("코인 충돌됨");

        if (other.tag == "Player" && !stepped) {
            stepped = true;
            //floatEffect.Play(1); // 코인 점수 효과 재생
            floatEffect.Play(coinCnt++); // 연속으로 코인을 먹을 때마다 점수가 증가하도록 함 📌
            GM.ins.AddScore(1);
            if (LM.ins != null) {
                LM.ins.AddCoin(1);
            }

            SM.ins.SfxAt(coinClip, transform.position);
            gameObject.SetActive(false);
        }
    }

}
