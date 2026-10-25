using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {

    [Header("키 설정 (PC)")]
    public Key attackKey = Key.A;
    public Key jumpKey = Key.Space;

    // 공격 : attackKey (기본 A)
    // (화면의 공격 버튼은 버튼 OnClick에서 Player.AttackByAKeyOrButton()을 직접 호출)
    public bool AttackDown {
        get {
            return Keyboard.current != null && Keyboard.current[attackKey].wasPressedThisFrame;
        }
    }

    // 점프 누름
    // - 모바일: 화면 터치로 점프 (터치는 마우스 왼쪽 버튼으로 인식됨)
    // - PC: jumpKey(기본 스페이스바)로만 점프, 마우스는 공격 버튼 클릭 전용 (마우스 클릭으로는 점프하지 않음)
    public bool JumpDown {
        get {
            if (Application.isMobilePlatform) {
                return Input.GetMouseButtonDown(0);
            }
            return Keyboard.current != null && Keyboard.current[jumpKey].wasPressedThisFrame;
        }
    }

    // 점프 뗌 (상승 중에 떼면 점프가 짧아짐)
    public bool JumpUp {
        get {
            if (Application.isMobilePlatform) {
                return Input.GetMouseButtonUp(0);
            }
            return Keyboard.current != null && Keyboard.current[jumpKey].wasReleasedThisFrame;
        }
    }
}
