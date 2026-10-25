using UnityEngine;
using AppsInToss;

public class LandscapeMode : MonoBehaviour {
    private async void Start() {
        try {
            await AIT.SetDeviceOrientation(new SetDeviceOrientationOptions {
                Type = SetDeviceOrientationOptionsType.Landscape
            });
        } catch (System.Exception e) {
            Debug.LogWarning("가로모드 설정 실패: " + e.Message);
        }
    }
}
