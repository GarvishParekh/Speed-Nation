using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header ("<size=15>SCRIPTABLE")]
    [SerializeField] private InputData inputData;
    // raw side ways input data
    private float sideInput;
    // lerped saw side ways data
    private float lerpedSideValue = 0;
    private float driftValue = 0;

    // updates on touch inputs
    bool isLeft = false;
    bool isRight = false;

    private void Awake()
    {
        inputData.lerpedSideValue = 0;
    }

    private void Update()
        => LerpedSideValue();

    float timer = 0.2f;
    private void LerpedSideValue()
    {
        switch (inputData.controls)
        {
            case Controls.KEYBOARD:
                sideInput = Input.GetAxisRaw("Horizontal");
                
                break;

            case Controls.TOUCH:
                if (isLeft) sideInput = -1;
                else if (isRight) sideInput = 1;
                else if (isRight == false && isLeft == false) sideInput = 0;
                break;
        }
        // input calculation
        if (sideInput == 0)
        {
            if (timer < 0) inputData.isPressed = false;
            else timer -= Time.deltaTime;
        }
        if (sideInput != 0)
        {
            timer = 0.15f;
            inputData.isPressed = true;
        }
        UpdateDriftValue(sideInput);

        lerpedSideValue = Mathf.Lerp(lerpedSideValue, sideInput, 2.1f * Time.deltaTime);

        inputData.sideValue = sideInput;
        inputData.lerpedSideValue = lerpedSideValue;
        inputData.driftrValue = driftValue;
    }

    private void UpdateDriftValue(float _sideValue)
    {
        if (_sideValue != 0) driftValue += Time.deltaTime;
        else if (_sideValue == 0) driftValue = 0;
    }

    #region Touch Inputs
    public void LeftTouchDown()
    {
        isLeft = true;
        isRight = false;    
    }

    public void LeftTouchUp()
        => isLeft = false;

    public void RightTouchDown()
    {
        isRight = true;
        isLeft = false;
    }

    public void RightTouchUp()
        => isRight = false; 
    #endregion

    public float GetSideInputs()
    {
        return sideInput;
    }

    public float GetLerpedSideInputs()
    {
        return lerpedSideValue;
    }
}
