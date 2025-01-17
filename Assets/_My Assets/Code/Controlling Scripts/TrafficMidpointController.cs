using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class TrafficMidpointController : MonoBehaviour, ITraffic
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
            // generating lane value Z
            randomLaneCount = Random.Range(0, trafficGeneratorData.lanes.Length);
            if (previousLaneCount == randomLaneCount)
            {
                if (randomLaneCount < trafficGeneratorData.lanes.Length - 1) randomLaneCount += 1;
                else randomLaneCount -= 1;
            }
            previousLaneCount = randomLaneCount;
            nextPlacingPosition.z = trafficGeneratorData.lanes[randomLaneCount];

            // generating distance value X 
            randomForwardPosition = Random.Range
            (
                trafficGeneratorData.minMaxDistance[(int)gameplayData.trafficLevel].x,
                trafficGeneratorData.minMaxDistance[(int)gameplayData.trafficLevel].y
            );
            xPosition -= randomForwardPosition;
            nextPlacingPosition.x = xPosition;

            // setting up cars according to values we generated
            allCars[i].localPosition = nextPlacingPosition;
            allCars[i].gameObject.SetActive(true);
        }

        // placing end point for spawned next seed
        nextPlacingPosition.z = 0;
        endPoint.localPosition = nextPlacingPosition;
        Debug.Log("Random traffic pattern generated: SUCESSFULL");
    }


    // ------------------  INTERFACE FUNCTIONS -------------------------------
    public Vector3 GetEndPointPosition()
    {
        return endPoint.position;
    }

    public void SetPosition(Vector3 newPosition) => transform.position = newPosition;

    public void Reset()
    {
        RandomPatternGenerator();
        Debug.Log("Reset of all cars: SUCESSFULL");
    }

    public Transform GetEndPointTransform()
    {
        return endPoint;
    }


    // ------------------  COLLISION BASE SPAWNING -------------------------------
    /* COLLISION BASE SPAWNING
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag ("Player")) 
            ActionManager.crossedMidPoint?.Invoke(endPoint);
    }
    */
}
