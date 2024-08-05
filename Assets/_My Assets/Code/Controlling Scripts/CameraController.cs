using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private CameraControllerData camData;

    [Header ("<size=15>SCRIPTS")]
    [SerializeField] private GameplayUiController gameplayUi;
    [SerializeField] private Transform cameraTransform;

    [Header ("<size=15>COMPONENTS")]
    [SerializeField] private Transform cameraInitialTransform;
    [SerializeField] private Transform cameraFinalTransform;

    [Header ("<size=15>USER INTERFACE")]
    [SerializeField] private GameObject upperImage;
    [SerializeField] private GameObject lowerImage;

    float cameraDamping;



    private void Awake()
    {
        cameraDamping = camData.maxCameraDamping;
        StartCoroutine(nameof(CameraPaning));    
    }

    private IEnumerator CameraPaning()
    {
        LeanTween.moveLocal(upperImage, Vector3.zero, 5).setEaseInOutSine();
        LeanTween.moveLocal(lowerImage, Vector3.zero, 5).setEaseInOutSine().setOnComplete(()=>
        {
            //gameplayUi.EnableControls();
        });

        cameraTransform.position = cameraInitialTransform.position;
        while (cameraTransform.position != cameraFinalTransform.position)
        {
            if (cameraDamping > camData.minDamingValue)
            {
                cameraDamping -= Time.deltaTime;
            }
            cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, cameraFinalTransform.position, cameraDamping * Time.deltaTime);
            yield return null;
        }

    }
}
