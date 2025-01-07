using TMPro;
using Google;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUiController : MonoBehaviour
{
    public static Action SettingsUpdated;

    UiManager uiManager;
    BgMusicManager bgMusicManager;
    GameCheckManager gameCheckManager;
    FirebaseInitlization firebaseInitilization;

    [SerializeField] private UpdateLeaderBoards updateLeaderboards;

    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    [SerializeField] private TMP_Text highscoreText;

    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private FirebaseData firebaseData;

    [Header("<size=15>UI EFFECTS")]
    [SerializeField] private GameObject arrowObject;

    [Header("<size=15>POST PROCESSING UI")]
    [SerializeField] private Toggle postProcessingToggle;
    [SerializeField] private TMP_Text serverConnectionText;

    [Header("<size=15>SIGIN UI ASSETS")]
    [Header("<size=10><I><color=#7DD2F6>GOOGLE")]
    [SerializeField] private GameObject googleSiginButton;
    [SerializeField] private GameObject googleLoggedin;
    [SerializeField] private GameObject googleLoginFailed;

    [Header("<size=10><I><color=#7DD2F6>APPLE")]
    [SerializeField] private GameObject appleSigninButton;
    [SerializeField] private GameObject appleLoggedIn;
    [SerializeField] private GameObject appleLoginFailed;

    [Header("<size=10><I><color=#7DD2F6>SETTINGS")]
    [SerializeField] private GameObject googleLoggingoff;
    [SerializeField] private GameObject appleLoggingOff;

    [Header("<size=15>MUSIC UI")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private TMP_InputField userNameInputFiled;
    [SerializeField] private Button backUsernameButton;
    [SerializeField] private Button confirmUserNameButton;
    [SerializeField] private TMP_Text userNameDisplayText;
    [SerializeField] private TMP_Text versionDisplayText;
    

    [Header("<size=15>SOUND EFFECTS UI")]
    [SerializeField] private Toggle soundEffectToggle;

    private void OnEnable()
    {
        FirebaseInitlization.ServerConnection += UpdateServerConnectionStatus;
        ActionManager.SignedInStatus += OnSignInStatusChange;
    }

    private void OnDisable()
    {
        FirebaseInitlization.ServerConnection -= UpdateServerConnectionStatus;
        ActionManager.SignedInStatus -= OnSignInStatusChange;
    }

    private void Awake() => CheckForDevice();

    private void Start()
    {
        firebaseInitilization = FirebaseInitlization.instance;
        gameCheckManager = GameCheckManager.instance;
        uiManager = UiManager.instance;
        bgMusicManager = BgMusicManager.instance;

        PreparingSettingsButton(firebaseData.signInType);
        CheckSignIn();


        if (firebaseInitilization.GetConnectionStatus())
        {
            serverConnectionText.text = "CONNECTED";
        }
        else
        {
            serverConnectionText.text = "CONNECTING...";
        }

        // check on player's username
        string userNameString = PlayerPrefs.GetString(ConstantKeys.USERNAME, "");
        if (userNameString == string.Empty) uiManager.OpenCanvasWithShutter(CanvasNames.ENTER_USER_NAME);
        else
        {
            userNameDisplayText.text = userNameString;
            uiManager.OpenCanvasWithShutter(CanvasNames.MAIN_MENU);

            if (!gameCheckManager.GetNewsSeen()) _OpenNewsCanvas();
            else uiManager.ClosePopUp(CanvasNames.NEWS_CANVAS);
        }

        LoadPostProcessing();
        LoadMusic();
        LoadSoundEffects();
        SetHighscore();
        ArrowAnimation();
        DisplayVersion();
    }

    // sign-in button control
    private void OnSignInStatusChange (SignInType signInType, bool check)
    {
        switch (signInType)
        {
            case SignInType.GOOGLE:
                if (check)
                {
                    PlayerPrefs.SetInt(ConstantKeys.LOGIN_STATUS, 1);
                    PlayerPrefs.SetInt(ConstantKeys.HAVE_LOGGEDIN, 1);

                    ButtonToShow(googleLoggedin);
                    googleLoggingoff.SetActive(true);
                }
                else ButtonToShow(googleLoginFailed);
                break;
            case SignInType.APPLE:
                if (check)
                {
                    PlayerPrefs.SetInt(ConstantKeys.LOGIN_STATUS, 1);
                    PlayerPrefs.SetInt(ConstantKeys.HAVE_LOGGEDIN, 1);

                    ButtonToShow(appleLoggedIn);
                    appleLoggedIn.SetActive(true);
                }
                else ButtonToShow(appleLoginFailed);
                break;
        }
        PreparingSettingsButton(signInType);
    }

    // settings login off button control
    private void PreparingSettingsButton(SignInType signInType)
    {
        int loginStatus = PlayerPrefs.GetInt(ConstantKeys.LOGIN_STATUS);
        switch (loginStatus)
        {
            case 1:
                switch (signInType)
                {
                    case SignInType.GOOGLE:
                        googleLoggingoff.SetActive(true);
                        break;

                    case SignInType.APPLE:
                        appleLoggedIn.SetActive(true);
                        break;
                }
                break;
        }
    }

    private void DisplayVersion()
    {
        versionDisplayText.text = $"v{Application.version}";
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
        GenericToggle(postProcessingToggle, ConstantKeys.POSTPROCESSING);
    }

    public void _MusicToggle()
    {
        GenericToggle(musicToggle, ConstantKeys.MUSIC);
    }

    public void _SoundToggle()
    {
        GenericToggle(soundEffectToggle, ConstantKeys.SOUNDS);
    }

    public void _ShopButton()
    {
        uiManager.OpenCanvasWithShutter(CanvasNames.SHOP);
    }

    public void GenericToggle(Toggle toggle, string tag)
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt(tag, 1);
        }
        else
        {
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

        if (!gameCheckManager.GetNewsSeen()) _OpenNewsCanvas();
        else uiManager.ClosePopUp(CanvasNames.NEWS_CANVAS);
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
        backUsernameButton.gameObject.SetActive(true);   
        confirmUserNameButton.gameObject.SetActive(false);   
        uiManager.OpenCanvasWithShutter(CanvasNames.ENTER_USER_NAME);
    }

    public void _OpenNewsCanvas()
    {
        uiManager.OpenPopUp(CanvasNames.NEWS_CANVAS);
    }

    public void _CloseNewsCanvas()
    {
        uiManager.ClosePopUp(CanvasNames.NEWS_CANVAS);
        gameCheckManager.SetNewsSeen(true); 
    }

    public void _CloseBuyCanvas()
    {
        uiManager.ClosePopUp(CanvasNames.BUY_CAR_CANVAS);
    }

    public void _CloseOutOfTicketCanvas()
    {
        uiManager.ClosePopUp(CanvasNames.OUT_OF_TICKETS_CANVAS);
    }

    public void _InitiateSignInProcess()
    {
        ActionManager.InitiateSignIn?.Invoke();
    }

    private void CheckSignIn()
    {
        int LoginStatus = PlayerPrefs.GetInt(ConstantKeys.LOGIN_STATUS, 0);

        switch (LoginStatus)
        {
            // not logged in 
            case 0:
#if UNITY_ANDROID
                ButtonToShow(googleSiginButton);
#elif UNITY_IPHONE
                ButtonToShow(appleSigninButton);
#endif
                break;

            // already logged in 
            case 1:
#if UNITY_ANDROID
                ButtonToShow(googleLoggedin);
#elif UNITY_IPHONE
                ButtonToShow(appleLoggedIn);
#endif
                break;
        }
    }

    private void ButtonToShow(GameObject buttonToShow)
    {
        // disable all buttons
        googleSiginButton.gameObject.SetActive(false);
        googleLoggedin.gameObject.SetActive(false);
        googleLoginFailed.gameObject.SetActive(false);

        appleSigninButton.gameObject.SetActive(false);
        appleLoggedIn.gameObject.SetActive(false);
        appleLoginFailed.gameObject.SetActive(false);

        // enable desire button
        buttonToShow.gameObject.SetActive(true);
    }

    public void _GoogleLogoutbutton()
    {
        GoogleSignIn.DefaultInstance.SignOut();
        PlayerPrefs.SetInt(ConstantKeys.LOGIN_STATUS, 0);
        CheckSignIn();

        appleLoggingOff.SetActive(false);
        googleLoggingoff.SetActive(false);
    }

    private void CheckForDevice()
    {
#if UNITY_ANDROID
        firebaseData.signInType = SignInType.GOOGLE;
#elif UNITY_IPHONE
        firebaseData.signInType = SignInType.APPLE;
#endif
    }
}
