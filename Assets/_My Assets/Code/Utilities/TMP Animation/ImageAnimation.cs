using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    public Image image; // Assign the Image component in the Inspector
    public float rotationDuration = 1f; // Duration for one full rotation
    public float jumpDuration = 1f; // Duration for the jump
    public float jumpHeight = 20f; // Height of the jump
    public float delayBetweenAnimations = 0.5f; // Delay between animation sets

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = image.transform.position;
        StartCoroutine(AnimateImage());
    }

    IEnumerator AnimateImage()
    {
        while (true)
        {
            // Rotate the image
            float rotationElapsedTime = 0f;
            while (rotationElapsedTime < rotationDuration)
            {
                float rotation = (rotationElapsedTime / rotationDuration) * 360f; // Complete rotation
                image.transform.rotation = Quaternion.Euler(0, rotation, 0);
                rotationElapsedTime += Time.deltaTime;
                yield return null;
            }
            image.transform.rotation = Quaternion.Euler(0, 0, 360f); // Ensure complete rotation

            // Perform the jump
            float jumpElapsedTime = 0f;
            while (jumpElapsedTime < jumpDuration)
            {
                float t = jumpElapsedTime / jumpDuration;
                float bounce = Mathf.Sin(t * Mathf.PI) * jumpHeight; // Jump effect
                image.transform.position = originalPosition + new Vector3(0, bounce, 0);
                jumpElapsedTime += Time.deltaTime;
                yield return null;
            }
            image.transform.position = originalPosition; // Ensure return to original position

            // Delay after jump
            yield return new WaitForSeconds(delayBetweenAnimations);
        }
    }
}
