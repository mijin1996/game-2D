using UnityEngine;

public class BackgroundLoop : MonoBehaviour {
    private float imgWidth;

    private void Awake() {
        // 가로 길이를 측정하는 처리
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        imgWidth = backgroundCollider.size.x;
    }

    private void Update() {
        // 현재 위치가 원점에서 왼쪽으로 imgWidth 이상 이동했을때 위치를 리셋
        if (transform.position.x <= -imgWidth) { Reposition(); }  //-width는 -100과 같음 && -로 갔기 때문에 repositioning해야함
    }

    // 위치를 리셋하는 메서드
    private void Reposition() {
        Vector2 offset = new Vector2(imgWidth * 3f, 0);
        transform.position = (Vector2)transform.position + offset;

    }
}
