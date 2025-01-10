using UnityEngine;

public class TrafficMidpointController : MonoBehaviour
{
    [SerializeField] private Transform endPoint;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag ("Player")) 
            ActionManager.crossedMidPoint?.Invoke(endPoint);
    }
}
