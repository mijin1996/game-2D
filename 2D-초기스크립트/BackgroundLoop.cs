using UnityEngine;

public class BackgroundLoop : MonoBehaviour {
    private float imgWidth;

    private void Awake() {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        imgWidth = backgroundCollider.size.x;
    }

    private void Update() {
        // 배경이 왼쪽으로 자기 가로 길이(imgWidth)만큼 지나가 화면 밖으로 완전히 나가면 (x <= -imgWidth)
        // 오른쪽 끝으로 옮겨서 배경이 끊기지 않고 이어지게 한다
        if (transform.position.x <= - imgWidth) {
            Reposition();
        }
    }

    private void Reposition() {
        Vector2 offset = new Vector2(imgWidth * 2f, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}
