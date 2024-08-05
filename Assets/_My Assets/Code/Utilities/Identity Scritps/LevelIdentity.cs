using UnityEngine;

public class LevelIdentity : MonoBehaviour
{
    [SerializeField] private LevelSpawnManager spawnManager;
    [SerializeField] private Transform levelTransform;
    [SerializeField] private Transform lastPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            spawnManager.SpawnAtNextPosition(levelTransform, lastPosition);
        }
    }
}
