using UnityEngine;

public class SfxManager : MonoBehaviour
{
    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private SfxData sfxData;
    [SerializeField] private AudioSource crashAudioSource;

    int crashTotalCount;

    private void OnEnable()
    {
        TrafficCarController.CarCollided += PlayCrashSfx;
    }

    private void OnDisable()
    {
        TrafficCarController.CarCollided -= PlayCrashSfx;
    }

    private void Awake()
    {
        crashTotalCount = sfxData.crashSfx.Length;
    }

    private void PlayCrashSfx()
    {
        int count = Random.Range(0, crashTotalCount);   
        crashAudioSource.PlayOneShot(sfxData.crashSfx[count]);
    }
}
