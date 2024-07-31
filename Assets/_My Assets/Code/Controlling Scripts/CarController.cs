using UnityEngine;
using System.Collections.Generic;


public class CarController : MonoBehaviour
{
    Rigidbody playerRb;

    [Header ("<size=15>COMPONENTS")]
    [Tooltip ("Apply velocity on the base of it's forward direction")]
    [SerializeField] private Transform rotationTransform;
    [Tooltip ("Rotation will directly apply on the disk")]
    [SerializeField] private Transform rotationDisk;
    [Tooltip ("Model of the vehicle including wheels")]
    [SerializeField] private Transform carModel;
    [SerializeField] private List<ParticleSystem> driftParticles;

    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private CarEngine engine;
    [SerializeField] private CarAnimationData carAnime;
    [SerializeField] private InputData inputData;

    [Header("<size=15>CAR ANIMATION")]
    [SerializeField] private List<Transform> frontWheels;
    [SerializeField] private List<Transform> allWheels;

    [Space]
    [SerializeField] private Transform carBody;


    // how much rotation vehicle is having - related to controls
    private float rotationValue = 0;
    // how much rotation vehicle's body is having - related to animation 
    private float currentBodyRotation = 0;
    // how much rotation vehicle's wheels are having - related to animation
    private float currentWheelRotation = 0;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        playerRb = GetComponent<Rigidbody>();   
    }

    private void FixedUpdate()
    {
        CarAcceleration();
        CarRotation();
        WheelRotation();
        BodyAnimation();
        DriftParticles();
    }

    private void CarAcceleration()
        => playerRb.velocity = rotationTransform.forward * engine.carSpeed;

    private void CarRotation()
    {
        rotationValue += inputData.lerpedSideValue * engine.turnSpeed * Time.deltaTime;
        rotationDisk.rotation = Quaternion.Euler(0, rotationValue, 0);

        carModel.rotation = rotationDisk.rotation;  
        rotationTransform.rotation = Quaternion.Lerp(rotationTransform.rotation, rotationDisk.rotation, engine.turnDamping * Time.deltaTime);
    }

    private void WheelRotation()
    {
        currentWheelRotation = Mathf.MoveTowards(currentWheelRotation, inputData.sideValue * carAnime.maxWheelRotation, carAnime.rotationDamping * Time.deltaTime);

        foreach (Transform wheel in frontWheels)
        {
            wheel.localRotation = Quaternion.Euler(0, currentWheelRotation, 0); ;
        }

        foreach (Transform wheel in allWheels)
        {
            wheel.Rotate(carAnime.wheelForwardRotation, 0, 0);
        }
    }

    
    

    private void DriftParticles()
    {
        if (inputData.driftrValue > 0.5f)
        {
            foreach (ParticleSystem particles in driftParticles)
            {
                particles.Play();
            }
        }
        else
        {
            foreach (ParticleSystem particles in driftParticles)
            {
                particles.Stop();
            }
        }
    }

    private void BodyAnimation()
    {
        currentBodyRotation = Mathf.MoveTowards(currentBodyRotation, inputData.sideValue * carAnime.maxBodyRotaion, carAnime.bodyRotationDamping * Time.deltaTime);
        carBody.localRotation = Quaternion.Euler(0, 0, currentBodyRotation);
    }

    
}
