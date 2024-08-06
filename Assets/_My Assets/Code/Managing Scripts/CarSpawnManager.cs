using UnityEngine;
using System.Collections.Generic;

public class CarSpawnManager : MonoBehaviour
{
    [Header ("<size=15>COMPONENTS")]
    [SerializeField] private Transform playerCar;
    [SerializeField] private List<Transform> TrafficCars = new List<Transform>();

    [Header("<size=15>VALUES")]
    [SerializeField] Vector2 spawnMinMaxTime;
    [SerializeField] float timer;

    Vector3 spawnPoint;

  

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;   
        }
        else
        {
            timer = Random.Range(spawnMinMaxTime.x, spawnMinMaxTime.y);
            spawnPoint.x = playerCar.position.x - 200;
            spawnPoint.y = 0.24f;
            float randomPosition = Random.Range(-107f , -113f);
            spawnPoint.z = randomPosition;

            int randomCar = Random.Range(0, TrafficCars.Count);
            Transform spawnedCar = Instantiate(TrafficCars[0], spawnPoint, Quaternion.Euler(0, 90, 0));

            TrafficCars.Remove(spawnedCar); 
            TrafficCars.Add(spawnedCar); 
        }
    }
}
