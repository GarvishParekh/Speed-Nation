
using UnityEngine;

public interface ITraffic
{
    public Vector3 GetEndPointPosition();
    public void SetPosition(Vector3 newPosition);
    public void Reset();
    public Transform GetEndPointTransform();
}
