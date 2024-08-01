using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GenericController : MonoBehaviour, ICanvasController
{
    CanvasGroup canvasGroup;

    private void Awake()
    {
         canvasGroup = GetComponent<CanvasGroup>();
    }
    public void DisableCanvas()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void EnableCanvas()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
