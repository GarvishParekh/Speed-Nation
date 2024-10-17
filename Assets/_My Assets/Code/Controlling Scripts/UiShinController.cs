using UnityEngine;

public class UiShinController : MonoBehaviour
{
    [SerializeField] private GameObject shinHolder;
    [SerializeField] private float endPoint;
    [SerializeField] private float animationDuration = 0.8f;
    Vector3 startingPos;

    private void Start()
    {
        startingPos = shinHolder.transform.localPosition;
        InvokeRepeating(nameof(ShinAnimation), 0, 5);
    }

    private void ShinAnimation()
    {
        shinHolder.transform.localPosition = startingPos;
        LeanTween.moveLocalX(shinHolder.gameObject, endPoint, animationDuration).setEaseInOutSine();
    }
}
