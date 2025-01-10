using UnityEngine;
using System.Collections.Generic;

public class TrafficSpawnMangerUpdated : MonoBehaviour
{
    [Header ("<size=15>SCRIPTABLE")]
    [SerializeField] private GameplayData gameplayData;

    [Header ("<size=15>SEEDS")]
    [SerializeField] private List<Transform> EasyTrafficSeeds = new List<Transform>();
    [SerializeField] private List<Transform> MediumTrafficSeeds = new List<Transform>();
    [SerializeField] private List<Transform> DifficultTrafficSeeds = new List<Transform>();

    private void OnEnable() => ActionManager.crossedMidPoint += OnCrossedMidPoint;

    private void OnDisable() => ActionManager.crossedMidPoint -= OnCrossedMidPoint;

    private void OnCrossedMidPoint(Transform spawnPoint)
    {
        Transform spawnedSeed = null;
        switch (GetTrafficLevel())
        {
            case TrafficLevel.EASY:
                spawnedSeed = EasyTrafficSeeds[0];
                spawnedSeed.GetComponent<ITraffic>().Reset();
                spawnedSeed.position = spawnPoint.position;
            break;
            
            case TrafficLevel.MEDIUM:

                break;
            case TrafficLevel.DIFFICULT:

                break;
        }
        EasyTrafficSeeds.Remove(spawnedSeed);
        EasyTrafficSeeds.Add(spawnedSeed);
    }

    int mediumScore;
    int DifficultScore;
    private TrafficLevel GetTrafficLevel()
    {
        mediumScore = gameplayData.mediumLevel;
        DifficultScore = gameplayData.difficultLevel;
            
        if (gameplayData.scoreCount < mediumScore)
            return TrafficLevel.EASY;

        //else if (gameplayData.scoreCount > mediumScore)
        else 
        {
            if (gameplayData.scoreCount > DifficultScore)
                return TrafficLevel.DIFFICULT;
            //else if (gameplayData.scoreCount < DifficultScore)
            else  return TrafficLevel.MEDIUM;
        }
    }
}
