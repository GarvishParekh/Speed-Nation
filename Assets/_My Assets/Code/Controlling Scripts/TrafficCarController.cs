using System;
using UnityEngine;

public class TrafficCarController : MonoBehaviour
{
    Rigidbody rb;
    bool wasCollided = false;

    [SerializeField] private GameObject afterCollision;
    [SerializeField] private TrafficSpawnManager carSpawnManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collided with player");
            ActionManager.CarCollided?.Invoke(transform);

            carSpawnManager.SpawnDestroyedCar(transform);
            gameObject.SetActive(false);
        }
    }
}
