using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum Controls
{
    KEYBOARD,
    TOUCH
}
public class CarController : MonoBehaviour
{
    Rigidbody playerRb;
    [SerializeField] private Controls controls;
    [SerializeField] private float playerSpeed = 2f;
    [SerializeField] private Transform rotationTransform;
    [SerializeField] private Transform rotationDisk;
    [SerializeField] private Transform carModel;
    [SerializeField] private float rotationValue = 0;
    [SerializeField] private float turnSpeed = 0;
    [SerializeField] private float rotationLerp = 0.25f;

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
    }

    private void CarAcceleration()
    {
        playerRb.velocity = rotationTransform.forward * playerSpeed;
    }

    float sideInput;
    private void CarRotation()
    {
        rotationValue += lerpedSideValue * turnSpeed * Time.deltaTime;

        carModel.rotation = rotationDisk.rotation;  
        rotationDisk.rotation = Quaternion.Euler(0, rotationValue, 0);
        rotationTransform.rotation = Quaternion.Lerp(rotationTransform.rotation, rotationDisk.rotation, rotationLerp * Time.deltaTime);
    }

    [Header ("<size=15>WHEEL ROTATION ANIMATION")]
    [SerializeField] private List<Transform> frontWheels;
    [SerializeField] private List<Transform> allWheels;
    [SerializeField] private float maxWheelRotation = 45f;
    [SerializeField] private float wheelAnimationTime = 250f;
    [SerializeField] private float wheelForwardRotation = 15f;
    float currentWheelRotation = 0;
    private void WheelRotation()
    {
        currentWheelRotation = Mathf.MoveTowards(currentWheelRotation, sideInput * maxWheelRotation, wheelAnimationTime * Time.deltaTime);

        foreach (Transform wheel in frontWheels)
        {
            wheel.localRotation = Quaternion.Euler(0, currentWheelRotation, 0); ;
        }

        foreach (Transform wheel in allWheels)
        {
            wheel.Rotate(wheelForwardRotation, 0, 0);
        }
    }

    float lerpedSideValue = 0;
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
        lerpedSideValue = Mathf.Lerp(lerpedSideValue, sideInput, 2.1f * Time.deltaTime);
    }

    [Header("<size=15>BODY ROTATION ANIMATION")]
    [SerializeField] private Transform carBody;
    [SerializeField] private float maxBodyRotaion = 45f;
    [SerializeField] private float bodyRotationTime = 250f;
    float currentBodyRotation = 0;
    private void BodyAnimation()
    {
        currentBodyRotation = Mathf.MoveTowards(currentBodyRotation, sideInput * maxBodyRotaion, bodyRotationTime * Time.deltaTime);
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
