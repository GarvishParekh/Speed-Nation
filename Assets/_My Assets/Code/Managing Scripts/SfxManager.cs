using UnityEngine;

public class SfxManager : MonoBehaviour
{
    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private GameSettingsData settingsData;
    [SerializeField] private SfxData sfxData;
    [SerializeField] private AudioSource crashAudioSource;
    [SerializeField] private AudioSource nosAudioSource;
    [SerializeField] private AudioSource boostCollectionAudioSource;
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
        ActionManager.CarCollided += PlayCrashSfx;
        ActionManager.PlayerBoosting += PlayNosSound;
        ActionManager.PlayerOnLane += PlayBoostCollectionSound;
    }

    private void OnDisable()
    {
        ActionManager.CarCollided -= PlayCrashSfx;
        ActionManager.PlayerBoosting -= PlayNosSound;
        ActionManager.PlayerOnLane -= PlayBoostCollectionSound;
    }

    private void Awake()
    {
        crashTotalCount = sfxData.crashSfx.Length;
    }

    private void PlayCrashSfx(Transform t)
    {
        int count = Random.Range(0, crashTotalCount);   
        crashAudioSource.PlayOneShot(sfxData.crashSfx[count]);
    }

    private void PlayNosSound(bool check)
    {
        if (check) nosAudioSource.PlayOneShot(sfxData.nosSfx);
    }

    private void PlayBoostCollectionSound()
    {
        boostCollectionAudioSource.PlayOneShot(sfxData.boostCollectionSfx);
    }
}
