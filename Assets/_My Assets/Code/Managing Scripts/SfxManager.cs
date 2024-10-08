using Unity.VisualScripting;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private GameSettingsData settingsData;
    [SerializeField] private SfxData sfxData;
    [SerializeField] private GameplayData gameplayData;

    [Header("<size=15>COMPONENTS")]
    [SerializeField] private AudioSource crashAudioSource;
    [SerializeField] private AudioSource nosAudioSource;
    [SerializeField] private AudioSource boostCollectionAudioSource;
    [SerializeField] private AudioSource killStreakAudioSource;
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
        ActionManager.UpdateKillStreak += OnUpdateKillStreak;
    }

    private void OnDisable()
    {
        ActionManager.CarCollided -= PlayCrashSfx;
        ActionManager.PlayerBoosting -= PlayNosSound;
        ActionManager.PlayerOnLane -= PlayBoostCollectionSound;
        ActionManager.UpdateKillStreak -= OnUpdateKillStreak;
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

    private void OnUpdateKillStreak(int count)
    {
        if (count == gameplayData.doubleKillValue) KillStreakplayer(sfxData.doubleKillSfx);
        else if (count == gameplayData.tripleKillValue) KillStreakplayer(sfxData.tripleKillSfx);
        else if (count == gameplayData.quadKillValue) KillStreakplayer(sfxData.quadKillSfx);
        else if (count == gameplayData.pentaCrushValue) KillStreakplayer(sfxData.pentaKillSfx);
        else if (count == gameplayData.rampageValue) KillStreakplayer(sfxData.rampageSfx);
        else if (count == gameplayData.godlikeValue) KillStreakplayer(sfxData.godlikeSfx);
        else if (count == gameplayData.unstopableValue) KillStreakplayer(sfxData.unstopableSfx);
    }

    private void KillStreakplayer(AudioClip clipToplay)
    {
        killStreakAudioSource.Stop();
        killStreakAudioSource.PlayOneShot(clipToplay);
    }
}
