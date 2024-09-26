using UnityEngine;

[CreateAssetMenu (fileName = "Sfx Data", menuName = "Scriptable / Sfx Data")]
public class SfxData : ScriptableObject
{
    public AudioClip[] crashSfx;
    public AudioClip nosSfx;
    public AudioClip boostCollectionSfx;
}
