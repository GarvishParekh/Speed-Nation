using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class CameraController : MonoBehaviour
{
    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private CameraControllerData camData;

    [Header ("<size=15>SCRIPTS")]
    [SerializeField] private GameplayUiController gameplayUi;
    [SerializeField] private Transform cameraTransform;

    [Header ("<size=15>COMPONENTS")]
    [SerializeField] private Transform playerCar;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform cameraInitialTransform;
    [SerializeField] private Transform cameraFinalTransform;

    [Header ("<size=15>USER INTERFACE")]
    [SerializeField] private GameObject upperImage;
    [SerializeField] private GameObject lowerImage;

    float cameraDamping;

    private void OnEnable()
    {
        TrafficCarController.CarCollided += OnCarCollision;
    }

    private void OnDisable()
    {
        TrafficCarController.CarCollided -= OnCarCollision;
    }

    private void Awake()
    {
        cameraDamping = camData.maxCameraDamping;
        StartCoroutine(nameof(CameraPaning));    
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        CollisionCamShake();
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

    private void FollowPlayer()
    {
        cameraHolder.position = playerCar.position;
        cameraHolder.rotation = Quaternion.Lerp(cameraHolder.rotation, playerCar.rotation , 5 * Time.deltaTime);    
    }

    public void CameraShake(float _intensityMultiplyer)
    {
        cameraTransform.localPosition = cameraFinalTransform.localPosition + Random.insideUnitSphere * camData.cameraShakeIntensity * _intensityMultiplyer;
    }

    float camShakeTimer = 0;
    private void CollisionCamShake()
    {
        if (camShakeTimer > 0)
        {
            CameraShake(3);
            camShakeTimer -= Time.deltaTime;    
        }
    }

    private void OnCarCollision()
        => camShakeTimer = camData.cameraShakeTime;

    public void SetPlayer(Transform _player)
    {
        playerCar = _player;
    }
}
