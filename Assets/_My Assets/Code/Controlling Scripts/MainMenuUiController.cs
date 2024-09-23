using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUiController : MonoBehaviour
{
    public static Action SettingsUpdated;

    FirebaseInitlization firebaseInitilization;
    UiManager uiManager;
    BgMusicManager bgMusicManager;
    [SerializeField] private UpdateLeaderBoards updateLeaderboards;

    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    [SerializeField] private TMP_Text highscoreText;

    [Header("<size=15>UI EFFECTS")]
    [SerializeField] private GameObject arrowObject;

    [Header("<size=15>POST PROCESSING UI")]
    [SerializeField] private Toggle postProcessingToggle;
    [SerializeField] private TMP_Text postProcessingText;
    [SerializeField] private TMP_Text serverConnectionText;

    [Header("<size=15>MUSIC UI")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private TMP_Text musicText;
    [SerializeField] private TMP_InputField userNameInputFiled;
    [SerializeField] private Button confirmUserNameButton;
    [SerializeField] private TMP_Text userNameDisplayText;
    

    [Header("<size=15>SOUND EFFECTS UI")]
    [SerializeField] private Toggle soundEffectToggle;
    [SerializeField] private TMP_Text soundEffectText;

    private void OnEnable()
    {
        FirebaseInitlization.ServerConnection += UpdateServerConnectionStatus;
    }

    private void OnDisable()
    {
        FirebaseInitlization.ServerConnection -= UpdateServerConnectionStatus;
        
    }

    private void Start()
    {
        firebaseInitilization = FirebaseInitlization.instance;

        if (firebaseInitilization.GetConnectionStatus())
        {
            serverConnectionText.text = "CONNECTED";
        }
        else
        {
            serverConnectionText.text = "CONNECTING...";
        }
        uiManager = UiManager.instance;
        bgMusicManager = BgMusicManager.instance;

        string userNameString = PlayerPrefs.GetString(ConstantKeys.USERNAME, "");

        if (userNameString == string.Empty) uiManager.OpenCanvasWithShutter(CanvasNames.ENTER_USER_NAME);
        else
        {
            userNameDisplayText.text = userNameString;
            uiManager.OpenCanvasWithShutter(CanvasNames.MAIN_MENU);
        }

        LoadPostProcessing();
        LoadMusic();
        LoadSoundEffects();
        SetHighscore();
        ArrowAnimation();
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

    private void ArrowAnimation()
    {
        LeanTween.moveLocalX(arrowObject, 1000, 1.5f).setLoopCount(-1);
    }


    public void _InstagramButton()
    {
        Application.OpenURL("https://www.instagram.com/ninesquare.games/");
    }

    string googleplayURL = "https://play.google.com/store/apps/details?id=com.theintellify.speednation&hl=en-IN";
    string appSoreURL = "https://apps.apple.com/pl/app/speed-nation/id6502994204";
    public void _RateMyGameButton()
    {
        #if UNITY_ANDROID
            Application.OpenURL(googleplayURL);

        #elif UNITY_IOS
            Application.OpenURL(appSoreURL);
        #else
            Application.OpenURL(googleplayURL);
        #endif
    }

    string discordURL = "https://discord.gg/QerZpCMhYs";
    public void _DiscordButton()
    {
        Application.OpenURL(discordURL);
    }

    public void _UpdateButton()
    {
    #if UNITY_ANDROID
                Application.OpenURL(googleplayURL);

    #elif UNITY_IOS
            Application.OpenURL(appSoreURL);
    #else
                Application.OpenURL(googleplayURL);
    #endif
    }

    public void SetHighscore()
    {
        int highscoreCount = PlayerPrefs.GetInt(ConstantKeys.HIGHSCORE, 0);
        highscoreText.text = "HIGHSCORE: " + highscoreCount.ToString("0");
    }

    public void _SetUserName()
    {
        PlayerPrefs.SetString(ConstantKeys.USERNAME, userNameInputFiled.text);

        userNameDisplayText.text = userNameInputFiled.text;
        uiManager.OpenCanvasWithShutter(CanvasNames.MAIN_MENU);
    }

    public void CheckForInputField()
    {
        if (userNameInputFiled.text.Length > 0) confirmUserNameButton.gameObject.SetActive(true);
        else confirmUserNameButton.gameObject.SetActive(false);
    }

    public void _OpenLeaderboards()
    {
        uiManager.OpenCanvasWithShutter(CanvasNames.LEADERBOARDS);
        updateLeaderboards.UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown (KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void UpdateServerConnectionStatus(bool sucess)
    {
        if (firebaseInitilization.GetConnectionStatus())
        {
            serverConnectionText.text = "CONNECTED";
            return;
        }
        if (sucess)
        {
            serverConnectionText.text = "CONNECTED";
        }
        else
        {
            serverConnectionText.text = "CONNECTION FAILED";
        }
    }

    public void _EditUserNameButton()
    {
        confirmUserNameButton.gameObject.SetActive(false);   
        uiManager.OpenCanvasWithShutter(CanvasNames.ENTER_USER_NAME);
    }
}
