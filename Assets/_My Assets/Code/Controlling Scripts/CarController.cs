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
    [SerializeField] private List<TrailRenderer> tireMarks;

    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private CarEngine engine;
    [SerializeField] private CarAnimationData carAnime;
    [SerializeField] private InputData inputData;

    [Space]
    [SerializeField] private Transform carBody;


    // how much rotation vehicle is having - related to controls
    private float rotationValue = -90;
    // how much rotation vehicle's body is having - related to animation 
    private float currentBodyRotation = 0;


    private void Awake()
    {
        rotationValue = -90;
        Application.targetFrameRate = 60;
        playerRb = GetComponent<Rigidbody>();   
    }

    private void FixedUpdate()
    {
        CarAcceleration();
        CarRotation();
        BodyAnimation();
        DriftParticles();
        AdjustCarSpeed();
    }

    public Vector3 collisionDirection = new Vector3 (0,0,0);
    private void CarAcceleration()
    {
        playerRb.velocity = rotationTransform.forward * engine.carSpeed + collisionDirection;
    }

    private void CarRotation()
    {
        // rotation of the disk 
        rotationValue += inputData.lerpedSideValue * engine.turnSpeed * Time.deltaTime;
        rotationDisk.rotation = Quaternion.Euler(0, rotationValue, 0);
        rotationValue = Mathf.Clamp(rotationValue, -135, -45);

        // rotation of the car model according to the disk
        carModel.rotation = rotationDisk.rotation;  

        // lerp the movement vecotor with rotation disk
        rotationTransform.rotation = Quaternion.Lerp
        (
            rotationTransform.rotation, 
            rotationDisk.rotation, 
            engine.turnDamping * Time.deltaTime
        );
    }

    // decrease the speed when the user turns
    private void AdjustCarSpeed()
    {
        engine.carSpeed = Mathf.Lerp
        (
            engine.maxCarSpeed, 
            engine.speedOnTurn, 
            Mathf.Abs(inputData.sideValue)
        );
    }


    // drift particles and tire marks if the vehicle turns after threshold
    private void DriftParticles()
    {
        if (inputData.driftrValue > engine.driftThreshold)
        {
            EmitDriftParticles(true);
            EmitTierMarks(true);
        }
        else
        {
            EmitDriftParticles(false);
            EmitTierMarks(false);
        }
    }

    private void EmitTierMarks(bool _check)
    {
        foreach (TrailRenderer tireMark in tireMarks)
        {
            tireMark.emitting = _check;
        }
    }

    private void EmitDriftParticles(bool _check)
    {
        if (_check)
        {
            foreach (ParticleSystem particles in driftParticles)
            {
                particles.Play(true);
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

    // body animation according to the turning of the player 
    private void BodyAnimation()
    {
        currentBodyRotation = Mathf.MoveTowards(currentBodyRotation, inputData.sideValue * carAnime.maxBodyRotaion, carAnime.bodyRotationDamping * Time.deltaTime);
        carBody.localRotation = Quaternion.Euler(0, 0, currentBodyRotation);
    }
}
