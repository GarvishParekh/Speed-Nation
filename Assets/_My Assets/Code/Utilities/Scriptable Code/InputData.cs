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

    [Space]
    public float sideValue;
    public float lerpedSideValue;
    public float driftrValue;

    public bool isPressed = false;
}
