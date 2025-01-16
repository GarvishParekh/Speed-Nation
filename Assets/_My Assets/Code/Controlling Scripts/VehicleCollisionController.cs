using UnityEngine;

public class VehicleCollisionController : MonoBehaviour
{
    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private GameplayData gameplayData;

    [Header("<size=15>SCRIPTS")]
    [SerializeField] private CameraController camController;

    [Header("<size=15>PARTICLES")]
    [SerializeField] private ParticleSystem crashLeft;
    [SerializeField] private ParticleSystem crashRight;

    [Header ("<size=15>COMPONENTS")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private bool isCrashing = false;

    [Header ("<size=15>VALUES")]
    [SerializeField] private float rayCastLenght;

    private void Start()
    {
        gameplayData.isCrashing = false;
        gameplayData.carCollisionTime = 0;
    }

    private void Update()
    {
        DetectCrash();
        CrashCounter();
    }

    private void DetectCrash()
    {
        // left 
        if (Physics.Raycast(transform.position, -transform.right, rayCastLenght, wallLayer))
        {
            crashLeft.gameObject.SetActive(true);
            camController.CameraShake(1);

            gameplayData.isCrashing = true;
            return;
        }
        /*
        else
        {
            crashLeft.gameObject.SetActive(false);
            inputData.isCrashing = false;
        }
        */

        // right
        else if (Physics.Raycast(transform.position, transform.right, rayCastLenght, wallLayer))
        {
            crashRight.gameObject.SetActive(true);
            camController.CameraShake(1);

            gameplayData.isCrashing = true;
            return;
        }
        else
        {
            crashRight.gameObject.SetActive(false);
            crashLeft.gameObject.SetActive(false);
            gameplayData.isCrashing = false;
        }
    }

    private void CrashCounter()
    {
        if (gameplayData.isCrashing)
        {
            gameplayData.carCollisionTime += Time.deltaTime;
        }
        else gameplayData.carCollisionTime = 0;

        if (gameplayData.carCollisionTime > gameplayData.carCollisionThreshold)
            ActionManager.HealthCompleted?.Invoke();
    }

    public void SetCameraController(CameraController _camController)
    {
        camController = _camController;
    }
}
