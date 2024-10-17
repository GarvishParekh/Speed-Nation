using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPlace;
    [SerializeField] private Vector3 levelLenght;

    public void SpawnAtNextPosition(Transform _level, Transform lastPosition)
    {
        _level.position = spawnPlace.position;
        spawnPlace.position = lastPosition.position;

    }
}
