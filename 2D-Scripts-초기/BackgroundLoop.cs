using UnityEngine;

public class BackgroundLoop : MonoBehaviour {
    private float imgWidth;

    private void Awake() {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        imgWidth = backgroundCollider.size.x;
    }

    private void Update() {
        if (transform.position.x <= - imgWidth) {
            Reposition();
        }
    }

    private void Reposition() {
        Vector2 offset = new Vector2(imgWidth * 2f, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}