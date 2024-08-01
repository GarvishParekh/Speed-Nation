using UnityEngine;

[CreateAssetMenu(fileName = "Camera Controller Data", menuName = "Scriptable / Camera Controller Data")]
public class CameraControllerData : ScriptableObject
{
    public float maxCameraDamping;
    public float minDamingValue = 2;
}
