using UnityEngine;

public class DestroyedVehicleParts : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private Vector3 defaultPosition;

    private void Awake()
        => rb = GetComponent<Rigidbody>();

    public void ResetPart()
    {
        rb.isKinematic = true;
        transform.localPosition = defaultPosition;  
        rb.isKinematic = false;
    }

}
