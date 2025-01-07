using System;
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
    [SerializeField] private List<ParticleSystem> boostPaticles;
    [SerializeField] private List<TrailRenderer> tireMarks;
    [SerializeField] private AudioSource engineSFX;

    [Header("<size=15>VALUES")]
    [SerializeField] private float speedMultiplier = 0;
    [SerializeField] private float boostingSpeed = 0;

    float boostingValue = 0;
    float boostingInputValue = 0;

    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private CarEngine engine;
    [SerializeField] private CarAnimationData carAnime;
    [SerializeField] private InputData inputData;
    [SerializeField] private GameSettingsData settingsData;

    [Space]
    [SerializeField] private Transform carBody;

    // how much rotation vehicle is having - related to controls
    private float rotationValue = -90;
    // how much rotation vehicle's body is having - related to animation 
    private float currentBodyRotation = 0;

    private void OnEnable()
    {
        ActionManager.PlayerBoosting += OnPlayerBoost;
    }

    private void OnDisable()
    {
        ActionManager.PlayerBoosting -= OnPlayerBoost;
    }

    private void Awake()
    {
        rotationValue = -90;
        Application.targetFrameRate = 60;
        playerRb = GetComponent<Rigidbody>();   

        if (settingsData.soundEffectsStatus == SoundEffectsStatus.OFF)
        {
            if (engineSFX != null)
            {
                engineSFX.enabled = false;
            }
        }
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
        float vehicleSpeed = engine.carSpeed + speedMultiplier + boostingInputValue;
        playerRb.velocity = rotationTransform.forward * vehicleSpeed + collisionDirection;
    }

    private void CarRotation()
    {
        if (inputData.isPressed)
        {
            switch (inputData.carControls)
            {
                case E_CarControls.LERPED:
                    rotationValue += inputData.lerpedSideValue * engine.turnSpeed * Time.deltaTime;
                break;
                case E_CarControls.RAW:
                    rotationValue += inputData.sideValue * engine.turnSpeed * Time.deltaTime;
                    break;
            }
        }
        else
        {
            rotationValue = Mathf.Lerp(rotationValue, -90, inputData.returnDamping * Time.deltaTime);
        }

        //rotationValue = Mathf.Clamp(rotationValue, -100, -80);
        rotationDisk.rotation = Quaternion.Euler(0, rotationValue, 0);

        // rotation of the car model according to the disk
        carModel.rotation = rotationDisk.rotation;

        switch (inputData.carControls)
        {
            case E_CarControls.LERPED:
                // lerp the movement vecotor with rotation disk
                rotationTransform.rotation = Quaternion.Lerp
                (
                    rotationTransform.rotation,
                    rotationDisk.rotation,
                    engine.turnDamping * Time.deltaTime
                );
            break;
            
            case E_CarControls.RAW:
                rotationTransform.rotation = rotationDisk.rotation;
            break;
        }
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
            EmitTierMarks(true);
        }
        else
        {
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

    // body animation according to the turning of the player 
    private void BodyAnimation()
    {
        currentBodyRotation = Mathf.MoveTowards(currentBodyRotation, inputData.sideValue * carAnime.maxBodyRotaion, carAnime.bodyRotationDamping * Time.deltaTime);
        carBody.localRotation = Quaternion.Euler(0, 0, currentBodyRotation);
    }

    public void SetRotationalDisk(Transform _rotationalDisk)
    {
        rotationDisk = _rotationalDisk;
    }

    public Transform GetRotationTransform()
    {
        return rotationTransform;
    }

    public static Action TollHit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag ("Toll"))
        {
            TollHit?.Invoke();
            speedMultiplier += 2;
        }
    }

    private void OnPlayerBoost(bool check)
    {
        if (check)
        {
            LeanTween.value(gameObject, boostingValue, boostingSpeed, 3).setOnUpdate((float newValue) =>
            {
                boostingValue = newValue; // Update the camFieldOfView as the tween progresses
                boostingInputValue = boostingValue;
            });

            foreach (ParticleSystem boostParticle in boostPaticles)
            {
                boostParticle.Play(true);
            }
        }
        else
        {
            LeanTween.value(gameObject, boostingValue, 0, 3).setOnUpdate((float newValue) =>
            {
                boostingValue = newValue; // Update the camFieldOfView as the tween progresses
                boostingInputValue = boostingValue;
            });
            foreach (ParticleSystem boostParticle in boostPaticles)
            {
                boostParticle.Stop(true);
            }
        }
    }
}
