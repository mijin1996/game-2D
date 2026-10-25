using UnityEngine;

public class Player : MonoBehaviour {
    public AudioClip deathClip;
    public float jumpForce = 700f;

    private int jumpCnt = 0;
    private bool isGrounded = false;
    private bool isDead = false;

    private Rigidbody2D rb;
    private Animator ani;
    private AudioSource pAudio;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        pAudio = GetComponent<AudioSource>();
    }

    private void Update() {
        if (isDead) {
            return;
        }

        if (Input.GetMouseButtonDown(0) && jumpCnt < 2) {
            jumpCnt++;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpForce));
            pAudio.Play();
        } else if (Input.GetMouseButtonUp(0) && rb.linearVelocity.y > 0) {
            rb.linearVelocity = rb.linearVelocity * 0.5f;
        }

        ani.SetBool("Grounded", isGrounded);
    }

    private void Die() {
        ani.SetTrigger("Die");

        pAudio.clip = deathClip;
        pAudio.Play();

        rb.linearVelocity = Vector2.zero;
        isDead = true;

        GM.gm.OnPlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Dead" && !isDead) {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.y > 0.7f) {
            isGrounded = true;
            jumpCnt = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        isGrounded = false;
    }
}