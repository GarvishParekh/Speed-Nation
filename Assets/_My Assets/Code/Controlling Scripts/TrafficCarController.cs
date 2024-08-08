using System;
using UnityEngine;

public class TrafficCarController : MonoBehaviour
{
    public static Action CarCollided;
    Rigidbody rb;
    bool wasCollided = false;

    [SerializeField] private GameObject afterCollision;
    [SerializeField] private CarSpawnManager carSpawnManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collided with player");
            CarCollided?.Invoke();

            carSpawnManager.SpawnDestroyedCar(transform);
            gameObject.SetActive(false);
        }
    }
}
