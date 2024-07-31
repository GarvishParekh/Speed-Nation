using UnityEngine;

[CreateAssetMenu(fileName = "Car Engine", menuName = "Scriptable / Create Car Engine")]
public class CarEngine : ScriptableObject
{
    [Header("<sizze=15>POWER")]
    public float carSpeed = 7.5f;
    public float maxCarSpeed = 7.5f;
    public float speedOnTurn = 7.5f;

    [Header("<sizze=15>HANDLING")]
    public float turnSpeed = 65f;
    public float turnDamping = 160f;
}
