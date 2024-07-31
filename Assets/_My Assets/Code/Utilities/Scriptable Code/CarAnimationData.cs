using UnityEngine;

[CreateAssetMenu (fileName = "Car Animation", menuName = "Scriptable / Create Car Animation")]
public class CarAnimationData : ScriptableObject
{
    [Header("<sizze=15>WHEEL ROTATION")]
    public float maxWheelRotation = 35f;
    public float rotationDamping = 250f;
    public float wheelForwardRotation = 13.5f;

    [Header("<size=15>BODY ROTATION ANIMATION")]
    public float maxBodyRotaion = 5.5f;
    public float bodyRotationDamping = 10f;
}
