using UnityEngine;

[CreateAssetMenu(fileName = "Camera Controller Data", menuName = "Scriptable / Camera Controller Data")]
public class CameraControllerData : ScriptableObject
{
    public float maxCameraDamping;
    public float minDamingValue = 2;
    public float cameraShakeIntensity = 0.05f;
    public float cameraShakeTime = 0.5f;
}
