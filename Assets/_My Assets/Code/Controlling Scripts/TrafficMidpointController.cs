using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class TrafficMidpointController : MonoBehaviour
{
    Rigidbody rb;

    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private GameplayData gameplayData;
    [SerializeField] private TrafficGeneratorData trafficGeneratorData;

    [Header("<size=15>COMPONENTS")]
    [SerializeField] private List<Transform> allCars = new List<Transform>();
    [SerializeField] private Transform endPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = gameplayData.trafficSpeed;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag ("Player")) 
            ActionManager.crossedMidPoint?.Invoke(endPoint);
    }

    [ContextMenu("ARRANGE CARS")]
    public void RandomPatternGenerator()
    {
        Vector3 nextPlacingPosition = Vector3.zero;
        float randomForwardPosition = 0;
        int randomLaneCount = 0;
        float xPosition = 0;
        int previousLaneCount = 0;

        for (int i = 0; i < allCars.Count; i++)
        {
            randomLaneCount = Random.Range(0, trafficGeneratorData.lanes.Length);
            if (previousLaneCount == randomLaneCount)
            {
                if (randomLaneCount < trafficGeneratorData.lanes.Length - 1) randomLaneCount += 1;
                else randomLaneCount -= 1;
            }
            previousLaneCount = randomLaneCount;
            randomForwardPosition = Random.Range
            (
                trafficGeneratorData.minMaxDistance.x,
                trafficGeneratorData.minMaxDistance.y
            );
            xPosition -= randomForwardPosition;

            nextPlacingPosition.z = trafficGeneratorData.lanes[randomLaneCount];
            nextPlacingPosition.x = xPosition;

            allCars[i].transform.localPosition = nextPlacingPosition;
        }
    }
}
