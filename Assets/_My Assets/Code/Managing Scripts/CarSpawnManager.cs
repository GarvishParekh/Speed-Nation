using UnityEngine;
using System.Collections.Generic;

public class CarSpawnManager : MonoBehaviour
{
    [Header ("<size=15>COMPONENTS")]
    [SerializeField] private Transform playerCar;
    [SerializeField] private Transform leftEdgeTransform;
    [SerializeField] private Transform rightEdgeTransform;
    [SerializeField] private List<Transform> TrafficCars = new List<Transform>();

    [Header("<size=15>VALUES")]
    [SerializeField] Vector2 spawnMinMaxTime;
    [SerializeField] float timer;

    Vector3 spawnPoint;
    Vector2 edgeValue;

    private void Awake()
    {
        edgeValue.x = leftEdgeTransform.position.z;
        edgeValue.y = rightEdgeTransform.position.z;
    }

    private void Start()
    {
        ShuffleList(TrafficCars);
    }

    private void Update()
    {
        if (timer > 0)
        {

            timer -= Time.deltaTime;
        }

        else
        {
            ResetTimer();
            spawnPoint.x = playerCar.position.x - 200;
            spawnPoint.y = 0.24f;
            spawnPoint.z = GetRandomNumber(edgeValue.x, edgeValue.y);

            Transform spawnedCar = TrafficCars[0];
            Rigidbody spawnedCarRb = spawnedCar.GetComponent<Rigidbody>();

            spawnedCar.position = spawnPoint;
            spawnedCar.rotation = Quaternion.Euler(0, 90, 0);

            float randomSpeed = Random.Range(20, 25);
            spawnedCarRb.velocity = spawnedCarRb.transform.forward * randomSpeed;

            UpdateTrafficList(spawnedCar);
        }
    }

    private void ResetTimer ()
        => timer = Random.Range(spawnMinMaxTime.x, spawnMinMaxTime.y);

    private float GetRandomNumber (float min, float max)
    {
        float randomPosition = Random.Range(min, max);
        return randomPosition;
    }

    private void UpdateTrafficList(Transform spawnedCar)
    {
        TrafficCars.Remove(spawnedCar);
        TrafficCars.Add(spawnedCar);
    }
    public void ShuffleList<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
