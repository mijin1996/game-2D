using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarrotProjectile : MonoBehaviour {
    [Header("당근 이동")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private int damage = 1;

    [Header("충돌 이펙트")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float hitEffectLifeTime = 0.5f;

    private Rigidbody2D rb;
    private Transform target;
    private bool hasHit;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    public void SetTarget(Transform newTarget) {
        target = newTarget;
        Vector2 direction;

        if (target != null) {
            direction = ((Vector2)target.position - rb.position).normalized;
        } else {
            direction = Vector2.right;
        }

        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // IDamageable을 구현한 적이면 종류와 상관없이 맞힘
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null) {
            return;
        }
        damageable.Damage(damage);
        if (hitEffectPrefab != null) {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, hitEffectLifeTime); // 당근 충돌 > 이팩트 > 1초대기 > 이펙트 제거
        }
        Destroy(gameObject);
    }
}
