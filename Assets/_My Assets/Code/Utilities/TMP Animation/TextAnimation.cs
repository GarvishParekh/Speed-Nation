using TMPro;
using UnityEngine;
using System.Collections;

public class TextAnimation : MonoBehaviour
{
    public TMP_Text messageText; // Assign the Text component in the Inspector
    public float swayDuration = 1f; // Duration for one sway cycle
    public float swayAmount = 5f; // Amount of horizontal sway

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = messageText.transform.position;
        StartCoroutine(SwayAnimation());
    }

    IEnumerator SwayAnimation()
    {
        while (true)
        {
            float elapsedTime = 0f;

            while (elapsedTime < swayDuration)
            {
                float t = Mathf.PingPong(elapsedTime / swayDuration, 1); // Smooth side-to-side motion
                float sway = Mathf.Sin(t * Mathf.PI * 2) * swayAmount; // Horizontal sway effect
                messageText.transform.position = originalPosition + new Vector3(sway, 0, 0);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}
