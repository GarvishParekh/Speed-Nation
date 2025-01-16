using UnityEngine;

[CreateAssetMenu(fileName = "Traffic Generator Data", menuName = "Scriptable / Traffic Generator Data")]
public class TrafficGeneratorData : ScriptableObject
{
    public Vector2 minMaxDistance;
    public float[] lanes;
}
