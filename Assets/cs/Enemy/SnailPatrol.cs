using UnityEngine;

public class SnailPatrol : MonoBehaviour, IDamageable {
    public float speed = 1f;

    private Vector3 startPos;
    private float moveDistance;
    private int direction = 1;
    private Animator ani;

    private bool isDead = false;
    private float originalSpeed;

    private void Awake() {
        ani = GetComponent<Animator>();
        originalSpeed = speed;
    }

    private void OnEnable() {// OnEnable :: 매번 호출
        direction = 1;
        isDead = false;
        speed = originalSpeed;
        if (ani != null) { ani.enabled = true; }

        moveDistance = GetComponentInParent<SpriteRenderer>().bounds.size.x / 2f;

        Vector3 pos = transform.localPosition;
        pos.x = Random.Range(-moveDistance, moveDistance);
        transform.localPosition = pos;

        startPos = transform.localPosition;
    }

    private void Update() {
        if (isDead) { return; }

        if (GM.ins != null && GM.ins.isGameover) { // 게임 오버 상태이면 달팽이 이동 중지 :: instance는 가르키는 대상:: 게임 안에 하나만 존재하는 gameobj=싱클톤
            ani.enabled = false;  return;
        }
        transform.localPosition += Vector3.right * direction * speed * Time.deltaTime; // 달팽이 현 위치 = (오른쪽 또는 왼쪽)으로 매 프레임 조금씩 이동시키는 코드

        float distance = transform.localPosition.x - startPos.x;

        if ((direction == 1 && distance >= moveDistance) || (direction == -1 && distance <= -moveDistance)) {//distance >= moveDistance || distance <= -moveDistance)
            direction *= -1;

        }
    }

    private void Flip() {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // IDamageable : 달팽이는 한 번 맞으면 바로 죽음 (원래 동작과 동일)
    public void Damage(int damage) {
        Die();
    }

    public void Die() {
        if (isDead) { return; }
        isDead = true;
        speed = 0f;
        // Destroy하면 발판이 재사용될 때 달팽이가 영영 사라지므로 비활성화만 한다
        gameObject.SetActive(false);
    }
}
