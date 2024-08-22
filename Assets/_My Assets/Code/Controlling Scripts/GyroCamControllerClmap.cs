using UnityEngine;

public class GyroCamControllerClmap : MonoBehaviour
{
    private Gyroscope gyro;
    private bool gyroSupported;

    public float rotationSpeed = 0.1f;
    public Vector2 pitchClamp = new Vector2(-60f, 60f); // Min and max pitch angles

    private Quaternion initialRotation;

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
            Quaternion newRotation = transform.rotation * rotationDelta;

            // Convert the rotation to Euler angles
            Vector3 eulerRotation = newRotation.eulerAngles;

            // Clamp the pitch (x-axis) while keeping yaw (y-axis) unchanged
            eulerRotation.x = ClampAngle(eulerRotation.x, pitchClamp.x, pitchClamp.y);
            newRotation = Quaternion.Euler(eulerRotation);

            // Apply the clamped rotation to the camera
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 10f * Time.deltaTime);
        }
    }

    // Helper function to clamp angles
    private float ClampAngle(float angle, float min, float max)
    {
        // Ensure angle is in the 0-360 range
        if (angle > 180f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
