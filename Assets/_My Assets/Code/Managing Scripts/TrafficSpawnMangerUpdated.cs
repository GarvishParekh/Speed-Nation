using UnityEngine;
using System.Collections.Generic;

public class TrafficSpawnMangerUpdated : MonoBehaviour
{
    [Header ("<size=15>SCRIPTABLE")]
    [SerializeField] private GameplayData gameplayData;

    [Header ("<size=15>COMPONENTS")]
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform carTransform;
    public float carDistance;

    [Header ("<size=15>SEEDS")]
    public List<TrafficMidpointController> trafficSeeds = new List<TrafficMidpointController>();

    int mediumScore;
    int DifficultScore;

    private void OnEnable()
    {
        ActionManager.PlayCarSpawned += OnCarSpawnn;
        //ActionManager.crossedMidPoint += ConfigureSeed;
    }

    private void OnDisable()
    {
        ActionManager.PlayCarSpawned -= OnCarSpawnn;
        //ActionManager.crossedMidPoint -= ConfigureSeed;
    }

    private void Start()
    {
        GetTrafficLevel();
        endPoint = trafficSeeds[trafficSeeds.Count - 1].GetEndPointTransform();
    }

    private void Update()
    {
        if (carTransform == null) return;
        SpawnNextSeed();
    }

    private void SpawnNextSeed()
    {
        carDistance = Vector3.Distance(endPoint.position, carTransform.position);
        if (carDistance <= 200) ConfigureSeed();
    }

    private void ConfigureSeed()
    {
        GetTrafficLevel();

        // get tail and head of the traffic seed
        TrafficMidpointController firstSeed = trafficSeeds[0];
        TrafficMidpointController lastSeed = trafficSeeds[trafficSeeds.Count - 1];

        // set position in-game
        firstSeed.SetPosition(lastSeed.GetEndPointPosition());
        firstSeed.Reset();

        // set position in list
        trafficSeeds.Remove(firstSeed);
        trafficSeeds.Add(firstSeed);

        endPoint = trafficSeeds[trafficSeeds.Count - 1].GetEndPointTransform();
    }

    private void GetTrafficLevel()
    {
        mediumScore = gameplayData.mediumLevel;
        DifficultScore = gameplayData.difficultLevel;
            
        if (gameplayData.scoreCount < mediumScore)
            gameplayData.trafficLevel = TrafficLevel.EASY;

        else 
        {
            if (gameplayData.scoreCount > DifficultScore)
                gameplayData.trafficLevel = TrafficLevel.DIFFICULT;

            else gameplayData.trafficLevel = TrafficLevel.MEDIUM;
        }
    }

    // ------------------ OBSERVING FUNCTION -------------------------------
    private void OnCarSpawnn(Transform spawnedCar) => carTransform = spawnedCar;
}
