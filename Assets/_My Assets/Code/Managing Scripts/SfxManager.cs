using UnityEngine;

public class SfxManager : MonoBehaviour
{
    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private GameSettingsData settingsData;
    [SerializeField] private SfxData sfxData;
    [SerializeField] private AudioSource crashAudioSource;
    [SerializeField] private GameObject enableStatus;

    int crashTotalCount;

    private void Start()
    {
        if (settingsData.soundEffectsStatus == SoundEffectsStatus.OFF)
        {
            enableStatus.SetActive(false);  
        }
    }

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
