using UnityEngine;

public class GyroCamController : MonoBehaviour
{
    private Gyroscope gyro;
    private bool gyroSupported;
    private Quaternion initialRotation;

    public float rotationSpeed = 0.1f;

    void Start()
    {
        gyroSupported = SystemInfo.supportsGyroscope;

        if (gyroSupported)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            initialRotation = transform.rotation;
        }
    }

    void Update()
    {
        if (gyroSupported)
        {
            // Get the rotation rate from the gyroscope
            Vector3 rotationRate = gyro.rotationRateUnbiased;

            // Calculate the new rotation based on the gyroscope data
            Quaternion rotationDelta = Quaternion.Euler(rotationRate.x * rotationSpeed, rotationRate.y * rotationSpeed, 0);

            // Apply the rotation to the camera
            transform.rotation = initialRotation * rotationDelta;
        }
    }
}

