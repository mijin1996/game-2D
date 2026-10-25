using UnityEngine;

public class Player : MonoBehaviour {

    [Header("사망")]
    public AudioClip deathClip;
    private bool isDead = false;

    [Header("점프")]
    public float jumpForce = 700f;
    public AudioClip jumpClip;
    public GameObject PlayerJumpEffectPrefab;
    private int jumpCnt = 0;
    private bool isGrounded = false;

    [Header("컴포넌트")]
    private Rigidbody2D rb;
    private Animator ani;
    private PlayerInput input;

    [Header("공격")]
    [SerializeField] private GameObject carrotPrefab;
    [SerializeField] private Transform carrotSpawnPoint;
    [SerializeField] private Transform attackTarget;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        if (input == null) { input = gameObject.AddComponent<PlayerInput>(); }
    }

    private void Update() {
        if (isDead) {
            return;
        }

        // 공격 입력 (A키)
        if (input.AttackDown) {
            Debug.Log("A키 입력 감지");
            AttackByAKeyOrButton();
        }

        // 점프 입력 (모바일: 터치 / PC: 스페이스바) → PlayerInput 참고
        bool jumpDown = input.JumpDown;
        bool jumpUp = input.JumpUp;

        // 점프 버튼을 눌렀으면 && 최대 점프 회수에 도달하지 않았다면
        if (jumpDown && jumpCnt < 5) {
            jumpCnt++;
            rb.linearVelocity = Vector2.zero; // 점프 직전에 속도를 순간적으로 제로(0, 0)로 변경
            rb.AddForce(new Vector2(0, jumpForce)); // 위로 점프 힘을 가함
            // 오디오 소스 재생
            SM.ins.PlayerSound(jumpClip); // 플레이어 전용 스피커 (원래처럼 새 점프 소리가 이전 소리를 끊음)
            if (PlayerJumpEffectPrefab != null) {
                Destroy(Instantiate(PlayerJumpEffectPrefab, transform.position, Quaternion.identity), 2f);
            }
        } else if (jumpUp && rb.linearVelocity.y > 0) {
            // 마우스 왼쪽 버튼에서 손을 뗐을 때 && 플레이어가 상승 중이라면
            // 상승 중인 경우 점프를 짧게 만들어주는 처리
            rb.linearVelocity = rb.linearVelocity * 0.5f;
        }

        // 애니메이터의 Grounded 파라미터를 isGrounded 값으로 갱신
        //ani.SetBool("Grounded", isGrounded);
    }

    public void AttackByAKeyOrButton() {
        if (isDead) { return; }
        Debug.Log("A키 또는 공격 버튼으로 공격 실행");
        ani.SetTrigger("Attack");
        if (carrotPrefab == null || carrotSpawnPoint == null) { return; }
        GameObject carrot = Instantiate(carrotPrefab, carrotSpawnPoint.position, Quaternion.identity);
        CarrotProjectile projectile = carrot.GetComponent<CarrotProjectile>();
        if (projectile != null) { projectile.SetTarget(attackTarget); }
    }

    private void Die() {
        ani.SetTrigger("Die");
        SM.ins.PlayerSound(deathClip); // 같은 스피커 → 사망 소리가 점프 소리를 끊음 (원래와 동일)

        // 속도를 제로(0,0)로 변경
        rb.linearVelocity = Vector2.zero;
        // 사망 상태로 true 로 변경
        isDead = true;
        GM.ins.OnPlayerDead();  //xx
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지 |> 충돌한 상대방의 태그가 Dead이며 아직 사망하지 않았다면 Die() 실행
        if (other.tag == "Dead" && !isDead) {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // 바닥에 닿았음을 감지하는 처리 |> 콜라이더와 닿았으며, 충돌 표면이 위쪽을 보고 있으면
        if (collision.contacts[0].normal.y > 0.7f) {
            // 바닥에 닿았음을 감지하는 처리
            isGrounded = true; // 바닥에 닿았으면 isGrounded를 true로 변경
            jumpCnt = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        isGrounded = false;
    }
}
