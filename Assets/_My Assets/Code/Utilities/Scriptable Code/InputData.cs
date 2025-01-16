using UnityEngine;

public enum Controls
{
    KEYBOARD,
    TOUCH
}

[CreateAssetMenu(fileName = "Input Data", menuName = ("Scriptable / Input Data"))]
public class InputData : ScriptableObject
{
    public Controls controls;
    public E_CarControls carControls;

    [Space]
    public float sideValue;
    public float lerpedSideValue;
    public float turnDamping = 5f;
    public float returnDamping = 1f;
    public float driftrValue;

    public bool isPressed = false;
}
