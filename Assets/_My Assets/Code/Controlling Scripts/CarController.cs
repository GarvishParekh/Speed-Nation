using UnityEngine;
using System.Collections.Generic;

public enum Controls
{
    KEYBOARD,
    TOUCH
}
public class CarController : MonoBehaviour
{
    Rigidbody playerRb;
    [SerializeField] private Controls controls;

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

    // raw side ways input data
    private float sideInput;
    // lerped saw side ways data
    private float lerpedSideValue = 0;
    private float driftValue = 0;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        playerRb = GetComponent<Rigidbody>();   
    }

    private void FixedUpdate()
    {
        LerpedSideValue();
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
        rotationValue += lerpedSideValue * engine.turnSpeed * Time.deltaTime;
        rotationDisk.rotation = Quaternion.Euler(0, rotationValue, 0);

        carModel.rotation = rotationDisk.rotation;  
        rotationTransform.rotation = Quaternion.Lerp(rotationTransform.rotation, rotationDisk.rotation, engine.turnDamping * Time.deltaTime);
    }

    private void WheelRotation()
    {
        currentWheelRotation = Mathf.MoveTowards(currentWheelRotation, sideInput * carAnime.maxWheelRotation, carAnime.rotationDamping * Time.deltaTime);

        foreach (Transform wheel in frontWheels)
        {
            wheel.localRotation = Quaternion.Euler(0, currentWheelRotation, 0); ;
        }

        foreach (Transform wheel in allWheels)
        {
            wheel.Rotate(carAnime.wheelForwardRotation, 0, 0);
        }
    }

    
    private void LerpedSideValue()
    {
        switch (controls)
        {
            case Controls.KEYBOARD:
                sideInput = Input.GetAxisRaw("Horizontal");
               
                    break;
            case Controls.TOUCH:
                if (isLeft)
                {
                    sideInput = -1;
                }
                else if (isRight)
                {
                    sideInput = 1;
                }
                else if (isRight == false && isLeft == false)
                {
                    sideInput = 0;
                }
                break;
        }
        if (sideInput != 0)
            driftValue += Time.deltaTime;
        else if (sideInput == 0)
            driftValue = 0;
        lerpedSideValue = Mathf.Lerp(lerpedSideValue, sideInput, 2.1f * Time.deltaTime);
    }

    private void DriftParticles()
    {
        if (driftValue > 0.5f)
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
        currentBodyRotation = Mathf.MoveTowards(currentBodyRotation, sideInput * carAnime.maxBodyRotaion, carAnime.bodyRotationDamping * Time.deltaTime);
        carBody.localRotation = Quaternion.Euler(0, 0, currentBodyRotation);
    }

    bool isLeft = false;
    bool isRight = false;
    public void LeftTouchDown()
    {
        isLeft = true;
    }

    public void LeftTouchUp()
    {
        isLeft = false;
    }

    public void RightTouchDown()
    {
        isRight = true;
    }

    public void RightTouchUp()
    {
        isRight = false;
    }
}
