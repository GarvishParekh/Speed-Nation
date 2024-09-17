using System.Collections;
using UnityEngine;
using TMPro;

public class PopOutTextEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float revealSpeed = 0.05f;
    public float popScale = 1.5f;
    public float popDuration = 0.2f;

    private void Start()
    {
        if (textMeshPro != null)
        {
            StartCoroutine(RevealTextWithPop());
        }
    }

    private IEnumerator RevealTextWithPop()
    {
        string fullText = textMeshPro.text;
        textMeshPro.text = string.Empty;

        for (int i = 0; i < fullText.Length; i++)
        {
            // Append the character to the text
            textMeshPro.text += fullText[i];

            // Get the character's transform
            Transform charTransform = textMeshPro.GetComponentInChildren<Transform>();

            // Animate the character
            StartCoroutine(PopCharacter(charTransform));

            // Wait for the reveal speed duration before revealing the next character
            yield return new WaitForSeconds(revealSpeed);
        }
    }

    private IEnumerator PopCharacter(Transform charTransform)
    {
        Vector3 originalScale = charTransform.localScale;
        Vector3 targetScale = originalScale * popScale;

        // Scale up (pop)
        float elapsedTime = 0f;
        while (elapsedTime < popDuration)
        {
            charTransform.localScale = Vector3.Lerp(originalScale, targetScale, (elapsedTime / popDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        charTransform.localScale = targetScale;

        // Scale down (return to original size)
        elapsedTime = 0f;
        while (elapsedTime < popDuration)
        {
            charTransform.localScale = Vector3.Lerp(targetScale, originalScale, (elapsedTime / popDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        charTransform.localScale = originalScale;
    }
}
