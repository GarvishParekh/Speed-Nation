using UnityEngine;

public class TrafficCarController : MonoBehaviour
{
    Rigidbody rb;
    bool wasCollided = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collided with player");
            rb.velocity = new Vector3(0, 30, 0);
            wasCollided = true;
        }
    }

    private void Update()
    {
        if (wasCollided)
        {
            transform.Rotate(15, 0, 0);
        }
    }

    public void ResetRotation()
    {
        wasCollided = false;
    }
}
