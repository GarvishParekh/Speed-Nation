using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    BgMusicManager musicManager;
    public static Action<bool, bool, bool> SettingsLoaded;
    [SerializeField] private GameSettingsData settingsData;

    [SerializeField] private GameObject highPostProcessing;
    [SerializeField] private GameObject lowPostProcessing;

    int postProcessingIndex = 0;
    int musicIndex = 0;
    int soundEffectIndex = 0;

    private void OnEnable()
    {
        MainMenuUiController.SettingsUpdated += OnSettingsUpdated;  
    }

    private void OnDisable()
    {
        MainMenuUiController.SettingsUpdated -= OnSettingsUpdated;  
    }

    private void Start()
    {
        musicManager = BgMusicManager.instance;
        LoadAllIndex();
    }

    // 1: ON | 0: OFF
    private void LoadAllIndex()
    {
        postProcessingIndex = PlayerPrefs.GetInt(ConstantKeys.POSTPROCESSING, 1);
        musicIndex = PlayerPrefs.GetInt(ConstantKeys.MUSIC, 1);
        soundEffectIndex = PlayerPrefs.GetInt(ConstantKeys.SOUNDS, 1);

        LoadSettings();
        ApplySettings();
    }

    private void LoadSettings()
    {
        if (postProcessingIndex == 0) settingsData.postProcessingStatus = PostProcessingStatus.LOW;
        else settingsData.postProcessingStatus = PostProcessingStatus.HIGH;

        if (musicIndex == 0) settingsData.musicStatus = MusicStatus.OFF;
        else settingsData.musicStatus = MusicStatus.ON;

        if (soundEffectIndex == 0) settingsData.soundEffectsStatus = SoundEffectsStatus.OFF;
        else settingsData.soundEffectsStatus = SoundEffectsStatus.ON;
    }

    private void ApplySettings()
    {
        switch (settingsData.postProcessingStatus)
        {
            case PostProcessingStatus.HIGH:
                lowPostProcessing.SetActive(false);
                highPostProcessing.SetActive(true);
            break;
            case PostProcessingStatus.LOW:
                lowPostProcessing.SetActive(true);
                highPostProcessing.SetActive(false);
            break;
        }

        switch (settingsData.musicStatus)
        {
            case MusicStatus.ON:
                musicManager.OnSettingsLoaded(true);
            break;
            case MusicStatus.OFF:
                musicManager.OnSettingsLoaded(false);
            break;
        }
    }

    private void OnSettingsUpdated()
        => LoadAllIndex();
}
