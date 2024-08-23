using System;
using UnityEngine;

public class FuelPickupController : MonoBehaviour
{
    public static Action FuelCollected;
    [SerializeField] private Transform fuelObject;

    [SerializeField] private FuelAnimationValue animeValues;
    

    private Vector3 startPosition;
    private float startY;

    void Start()
    {
        startPosition = fuelObject.localPosition;
        startY = transform.position.y;
    }

    void Update()
    {
        // Levitating
        float newY = startY + Mathf.Sin(Time.time * animeValues.levitationSpeed) * animeValues.levitationHeight + animeValues.levitationHeightOffest;
        fuelObject.localPosition = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotating
        transform.Rotate(Vector3.up, animeValues.rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FuelCollected?.Invoke();
            gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class FuelAnimationValue
{
    public float levitationHeight = 0.3f;
    public float levitationHeightOffest = 0.54f;
    public float levitationSpeed = 2.15f;
    public float rotationSpeed = 30.0f;
}
