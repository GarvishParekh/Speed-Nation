using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPlace;
    [SerializeField] private Vector3 levelLenght;

    [Space]
    [SerializeField] private List<Transform> fuelSpawnPoints = new List<Transform>();
    [SerializeField] private GameObject fuelPrefab;
    [SerializeField] private int fuelSpawnCount = 0;
    [SerializeField] private Transform fuelCollectedParticles;

    [SerializeField] private bool canpawnFuel = false;

    public void SpawnAtNextPosition(Transform _level, Transform lastPosition)
    {
        fuelSpawnCount++;
        _level.position = spawnPlace.position;
        spawnPlace.position = lastPosition.position;

        if (!canpawnFuel) return;

        if (fuelSpawnCount > 10)
        {
            SpawnFuel();
            fuelSpawnCount = 0;
        }
    }

    private void SpawnFuel()
    {
        fuelPrefab.SetActive(true);
        int randomSpawnIndex = Random.Range(0, fuelSpawnPoints.Count);

        Vector3 spawnPoition = fuelSpawnPoints[randomSpawnIndex].position;
        fuelPrefab.transform.position = spawnPoition;
        fuelCollectedParticles.position = spawnPoition;
    }
}
