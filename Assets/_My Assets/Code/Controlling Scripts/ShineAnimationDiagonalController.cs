using UnityEngine;

public class ShineAnimationDiagonalController : MonoBehaviour
{
    [SerializeField] private GameObject shineObj;
    [SerializeField] private Transform endPositionObj;
    [SerializeField] private float animationSpeed;

    [Space]
    [SerializeField] private float startingOffset = 0;
    [SerializeField] private float repeatingRate;

    Vector3 shineStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        shineStartPosition = shineObj.transform.localPosition;
        InvokeRepeating(nameof(ShineAnimation), 0, repeatingRate);
    }


    private void ShineAnimation()
    {
        shineObj.transform.localPosition = shineStartPosition;
        LeanTween.moveLocal(shineObj, endPositionObj.localPosition, animationSpeed);
    }
}
