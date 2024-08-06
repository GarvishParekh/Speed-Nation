using UnityEngine;

public class VehicleCollisionController : MonoBehaviour
{
    [Header("<size=15>PARTICLES")]
    [SerializeField] private ParticleSystem crashLeft;
    [SerializeField] private ParticleSystem crashRight;

    [Header ("<size=15>COMPONENTS")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform originalTransform;
    [SerializeField] private LayerMask wallLayer;

    [Header ("<size=15>VALUES")]
    [SerializeField] private float rayCastLenght;
    [SerializeField] private float shakeIntensity;
    
    private void Update()
    {
        Debug.DrawRay(transform.position, -transform.right * rayCastLenght, Color.red);

        // left 
        if (Physics.Raycast(transform.position, -transform.right, rayCastLenght, wallLayer))
        {
            crashLeft.gameObject.SetActive(true);
            camTransform.localPosition = originalTransform.localPosition + Random.insideUnitSphere * shakeIntensity;
        }
        else crashLeft.gameObject.SetActive(false);

        // right
        if (Physics.Raycast(transform.position, transform.right, rayCastLenght, wallLayer))
        {
            crashRight.gameObject.SetActive(true);
            camTransform.localPosition = originalTransform.localPosition + Random.insideUnitSphere * shakeIntensity;
        }
        else crashRight.gameObject.SetActive(false);
    }
}
