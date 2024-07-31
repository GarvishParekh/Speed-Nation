using UnityEngine;

public enum WheelPosition
{
    FRONT,
    REAR
}
public class WheelIdentity : MonoBehaviour
{
    [SerializeField] private WheelPosition wheelPosition;

    public WheelPosition GetWheelPosition()
    {
        return wheelPosition;
    }
}
