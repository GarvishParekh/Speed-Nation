using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUiController : MonoBehaviour
{
    public static Action SettingsUpdated;

    UiManager uiManager;
    BgMusicManager bgMusicManager;

    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    [Header("<size=15>POST PROCESSING UI")]
    [SerializeField] private Toggle postProcessingToggle;
    [SerializeField] private TMP_Text postProcessingText;

    [Header("<size=15>MUSIC UI")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private TMP_Text musicText;

    [Header("<size=15>SOUND EFFECTS UI")]
    [SerializeField] private Toggle soundEffectToggle;
    [SerializeField] private TMP_Text soundEffectText;

    private void Start()
    {
        uiManager = UiManager.instance;
        bgMusicManager = BgMusicManager.instance;

        uiManager.OpenCanvasWithShutter(CanvasNames.MAIN_MENU);

        LoadPostProcessing();
        LoadMusic();
        LoadSoundEffects();
    }

    public void _LetsGoButton()
    {
        bgMusicManager.StartMusic();
        uiManager.StartSceneChangeRoutine(ConstantKeys.SCENE_GAMEPLAY);
    }
    public void _DriveButton()
    {
        uiManager.OpenCanvasWithShutter(CanvasNames.LEVEL_SELECTION);
    }
    public void _PracticeButton()
    {
        uiManager.StartSceneChangeRoutine(ConstantKeys.SCENE_PRACTICE);
    }

    public void _GrageButton()
    {
        uiManager.OpenCanvasWithShutter(CanvasNames.GARAGE);
    }

    public void _MainMenuButton()
    {
        uiManager.OpenCanvasWithShutter(CanvasNames.MAIN_MENU);
    }

    public void _OpenSettingsButton()
    {
        uiManager.OpenCanvasWithShutter(CanvasNames.OPTIONS);
    }

    public void _CloseSettingsButton()
    {
        uiManager.OpenCanvasWithShutter(CanvasNames.MAIN_MENU);
    }

    public void _PostProcessingToggle()
    {
        GenericToggle(postProcessingToggle, postProcessingText, "Post Processing: High", "Post Processing: Low", ConstantKeys.POSTPROCESSING);
    }

    public void _MusicToggle()
    {
        GenericToggle(musicToggle, musicText, "Music: On", "Music: Off", ConstantKeys.MUSIC);
    }

    public void _SoundToggle()
    {
        GenericToggle(soundEffectToggle, soundEffectText, "Sound Effects: On", "Sound Effects: Off", ConstantKeys.SOUNDS);
    }

    public void _ShopButton()
    {
        uiManager.OpenCanvasWithShutter(CanvasNames.SHOP);
    }

    public void GenericToggle(Toggle toggle, TMP_Text displapText, string onString, string offString, string tag)
    {
        if (toggle.isOn)
        {
            displapText.text = onString;
            displapText.color = onColor;
            PlayerPrefs.SetInt(tag, 1);
        }
        else
        {
            displapText.text = offString;
            displapText.color = offColor;
            PlayerPrefs.SetInt(tag, 0);
        }

        SettingsUpdated?.Invoke();
    }

    private void LoadPostProcessing()
    {
        int toggleInt = PlayerPrefs.GetInt(ConstantKeys.POSTPROCESSING, 1);

        if (toggleInt == 0)
            postProcessingToggle.isOn = false;
        if (toggleInt == 1)
            postProcessingToggle.isOn = true;

        _PostProcessingToggle();
    }

    private void LoadMusic()
    {
        int toggleInt = PlayerPrefs.GetInt(ConstantKeys.MUSIC, 1);

        if (toggleInt == 0)
            musicToggle.isOn = false;
        if (toggleInt == 1)
            musicToggle.isOn = true;

        _MusicToggle();
    }

    private void LoadSoundEffects()
    {
        int toggleInt = PlayerPrefs.GetInt(ConstantKeys.SOUNDS, 1);

        if (toggleInt == 0)
            soundEffectToggle.isOn = false;
        if (toggleInt == 1)
            soundEffectToggle.isOn = true;

        _SoundToggle();
    }

    public void _InstagramButton()
    {
        Application.OpenURL("https://www.instagram.com/ninesquare.games/");
    }

    string googleplayURL = "https://play.google.com/store/apps/details?id=com.theintellify.speednation&hl=en-IN";
    public void _RateMyGameButton()
    {
        Application.OpenURL(googleplayURL);
    }

    string discordURL = "https://discord.gg/g6ktYQZZ";
    public void _DiscordButton()
    {
        Application.OpenURL(discordURL);
    }
}
