using UnityEngine;

public enum Crash
{
    ON,
    OFF
}
public class VehicleCollisionController : MonoBehaviour
{
    Rigidbody carRb;
    public Crash carsh;
    public CarController carController;
    public InputData inputData;
    public float collisionDirZ;
    public float collisionTimer = 0;
    public float collisionTime = 0.2f;
    public float impactForce = 0.5f;

    private void Awake()
    {
        carRb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.collider.name);
        if (collision.collider.CompareTag("Wall"))
        {
            carsh = Crash.ON;
            Debug.Log("Collision");
            Vector3 collisionForce = collision.contacts[0].normal * impactForce;
            carRb.AddForce(collisionForce, ForceMode.VelocityChange);
            //collisionTimer = collisionTime;
        }
    }

    /*
    private void Update()
    {
        if (collisionTimer > 0)
        {
            carController.collisionDirection = new Vector3(0, 0, impactForce * collisionTimer);
            collisionTimer -= Time.deltaTime;
        }
        else if (collisionTimer <= 0) 
        {
            carController.collisionDirection = new Vector3(0, 0, 0);
            carsh = Crash.OFF;
        }
    }
    */
}
