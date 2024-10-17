using UnityEngine;

[CreateAssetMenu (fileName = "Sfx Data", menuName = "Scriptable / Sfx Data")]
public class SfxData : ScriptableObject
{
    public AudioClip[] crashSfx;
    public AudioClip nosSfx;
    public AudioClip boostCollectionSfx;

    [Header ("<size=15>KILL STREAK")]
    public AudioClip doubleKillSfx;
    public AudioClip tripleKillSfx;
    public AudioClip quadKillSfx;
    public AudioClip pentaKillSfx;
    public AudioClip rampageSfx;
    public AudioClip godlikeSfx;
    public AudioClip unstopableSfx;
}
