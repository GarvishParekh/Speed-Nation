using UnityEngine;

public class VehicleCollisionController : MonoBehaviour
{
    [Header("<size=15>SCRIPTS")]
    [SerializeField] private CameraController camController;

    [Header("<size=15>PARTICLES")]
    [SerializeField] private ParticleSystem crashLeft;
    [SerializeField] private ParticleSystem crashRight;

    [Header ("<size=15>COMPONENTS")]
    [SerializeField] private LayerMask wallLayer;

    [Header ("<size=15>VALUES")]
    [SerializeField] private float rayCastLenght;
    
    private void Update()
    {
        Debug.DrawRay(transform.position, -transform.right * rayCastLenght, Color.red);

        // left 
        if (Physics.Raycast(transform.position, -transform.right, rayCastLenght, wallLayer))
        {
            crashLeft.gameObject.SetActive(true);
            camController.CameraShake(1);
        }
        else crashLeft.gameObject.SetActive(false);

        // right
        if (Physics.Raycast(transform.position, transform.right, rayCastLenght, wallLayer))
        {
            crashRight.gameObject.SetActive(true);
            camController.CameraShake(1);
        }
        else crashRight.gameObject.SetActive(false);
    }

    public void SetCameraController(CameraController _camController)
    {
        camController = _camController;
    }
}
