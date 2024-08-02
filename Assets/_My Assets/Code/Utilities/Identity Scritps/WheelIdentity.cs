using UnityEngine;

public enum WheelPosition
{
    FRONT,
    REAR
}
public class WheelIdentity : MonoBehaviour
{
    [Header ("<size=15>SCRIPTABLE")]
    [SerializeField] private InputData inputData;
    [SerializeField] private CarAnimationData carAnime;

    [Header ("<size=15>CHECKS")]
    [SerializeField] private WheelPosition wheelPosition;
    [SerializeField] private bool isNegative = false;

    private float currentWheelRotation;

    private void Update()
        => WheelRotation();

    private void WheelRotation()
    {
        // calculation for front wheel side ways rotation 
        currentWheelRotation = Mathf.MoveTowards
        (
            currentWheelRotation,
            inputData.sideValue * carAnime.maxWheelRotation,
            carAnime.rotationDamping * Time.deltaTime
        );

        switch (wheelPosition)
        {
            // applying wheel forward and side ways rotation
            case WheelPosition.FRONT:
                transform.localRotation =
                    Quaternion.Euler(0, currentWheelRotation, 0);
                if (isNegative)
                    transform.GetChild(0).Rotate(-carAnime.wheelForwardRotation, 0, 0);
                else 
                    transform.GetChild(0).Rotate(carAnime.wheelForwardRotation, 0, 0);
                break;

            // applying wheel forward 
            case WheelPosition.REAR:
                if (isNegative)
                    transform.Rotate(-carAnime.wheelForwardRotation, 0, 0);
                else
                    transform.Rotate(carAnime.wheelForwardRotation, 0, 0);

                break;
        }
    }
}
