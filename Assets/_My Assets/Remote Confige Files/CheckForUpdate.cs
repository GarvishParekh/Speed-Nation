using UnityEngine;

using Unity.Services.Core;
using Unity.Services.RemoteConfig;

using System.Threading.Tasks;
using Unity.Services.Authentication;

public class CheckForUpdate : MonoBehaviour
{
    //private Review
    [Header("<size=15>[SCRIPTS]")]
    public static CheckForUpdate instance;
    UiManager uiManager;

    public string myGameVersion;

    public bool update_Check_RC = true;
    public string android_Version_RC;
    public string iOS_Version_RC;

    string retrivedVersion;

    public struct userAttributes { }

    public struct appAttributes
    {
        public bool updateCheck;
        public string androidAppVersion;
        public string iOSAppVersion;
    }

    async Task InitilizeRemoteConfigAsync()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        myGameVersion = Application.version;
        DontDestroyOnLoad(gameObject);  
    }

    async void Start()
    {
        uiManager = UiManager.instance;
        await InitilizeRemoteConfigAsync();

        appAttributes aaStruct = new appAttributes();

        RemoteConfigService.Instance.FetchConfigs<userAttributes, appAttributes>(new userAttributes() { }, aaStruct);

        RemoteConfigService.Instance.FetchCompleted += RemoteConfigLoaded;
    }

    private void RemoteConfigLoaded (ConfigResponse configResponse)
    {
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Remote:

                update_Check_RC = RemoteConfigService.Instance.appConfig.GetBool("Version_Check");
                if (update_Check_RC)
                {
#if UNITY_IPHONE
                    iOS_Version_RC = RemoteConfigService.Instance.appConfig.GetString("iOS_Version");
                    retrivedVersion = iOS_Version_RC;

#elif UNITY_ANDROID
                    android_Version_RC = RemoteConfigService.Instance.appConfig.GetString("Android_Version");
                    retrivedVersion = android_Version_RC;

# endif

                    if (retrivedVersion != myGameVersion)
                    {
                        Debug.Log("Version Not matched");
                        uiManager.OpenCanvasWithShutter(CanvasNames.UPDATE);
                    }
                    else if (retrivedVersion == myGameVersion)
                    {
                        Debug.Log("Update-to-date");
                    }
                    Debug.Log("Version check complete");
                }
            break;
        }
    }
}
