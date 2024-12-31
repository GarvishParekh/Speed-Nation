using System; 
using Firebase;
using UnityEngine;
using Firebase.Auth;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Crashlytics;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

#if UNITY_IPHONE
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
#endif

#if UNITY_ANDROID
using Google;
#endif

public class FirebaseInitlization : MonoBehaviour
{
    [Header ("SCRIPTABLE")] 
    [SerializeField] private FirebaseData firebaseData;
    public static Action<bool> ServerConnection;

    #if UNITY_IPHONE
    // for apple login
    private IAppleAuthManager appleAuthManager;
#endif

    private FirebaseUser user;

    public static FirebaseInitlization instance;
    public FirebaseAuth auth;

    [SerializeField] private List<string> leaderboardsNameData;
    [SerializeField] private List<int> leaderboardsScoreData;
    [SerializeField] private int myRank = 0;

    public bool canUseFirestore = false;

    string encryptDeviceID;

    private void OnEnable()
    {
        ActionManager.InitiateSignIn += OnInitiateSignInProcess;
    }

    private void OnDisable()
    {
        ActionManager.InitiateSignIn -= OnInitiateSignInProcess;
    }

    private void OnInitiateSignInProcess() => InitializeAuthFirebase();

    private void Awake()
    {
        string testString = HashingHelper.GetSha256Hash("Garvish");
        Debug.Log($"HASH KEY: {testString}");
        CreateSingleton();
    }

    private void CreateSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
#if UNITY_IPHONE
        // If the current platform is supported
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            this.appleAuthManager = new AppleAuthManager(deserializer);
        }
#endif
        encryptDeviceID = HashingHelper.GetSha256Hash(SystemInfo.deviceUniqueIdentifier);
        FirestoreInitializer(sucess =>
        {
            if (sucess)
            {
                ServerConnection?.Invoke(true);
                canUseFirestore = true;
                FirebaseAnalyticsInitilization(sucess =>
                {
                    if (sucess)
                    {
                        //InitializeAuthFirebase();
                    }
                });
            }
            else
            {
                ServerConnection?.Invoke(false);
                canUseFirestore = false;
            }
        });
    }

    private void Update()
    {
#if UNITY_IPHONE
        // Updates the AppleAuthManager instance to execute
        // pending callbacks inside Unity's execution loop
        if (appleAuthManager != null)
        {
            appleAuthManager.Update();
        }
#endif
    }

    private void FirebaseAnalyticsInitilization(Action<bool> onComplete)
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                // Crashlytics will use the DefaultInstance, as well;
                // this ensures that Crashlytics is initialized.
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

                // When this property is set to true, Crashlytics will report all
                // uncaught exceptions as fatal events. This is the recommended behavior.
                Crashlytics.ReportUncaughtExceptionsAsFatal = true;

                // Set a flag here for indicating that your project is ready to use Firebase.
                onComplete?.Invoke(true);  // Notify success
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                onComplete?.Invoke(false);  // Notify success
            }
        });
    }

    private FirebaseFirestore firestore;
    private void FirestoreInitializer(Action<bool> onComplete)
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Firebase.DependencyStatus dependencyStatus = task.Result;

            Debug.Log(task.Result);

            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Firebase is ready, initialize Firestore
                firestore = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firestore successfully initialized.");
                onComplete?.Invoke(true);  // Notify success
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                onComplete?.Invoke(false);  // Notify failure
            }
        });
    }

    string userName = "Garvish";
    int scoreCount = 500;
    public void UpdateHighscoreOnServer(Action<bool> onComplete)
    {
        if (!canUseFirestore)
        {
            onComplete?.Invoke(false);  // Notify success
            return;
        }
        userName = PlayerPrefs.GetString(ConstantKeys.USERNAME);
        scoreCount = PlayerPrefs.GetInt(ConstantKeys.HIGHSCORE, 0);
        
        try
        {
            DocumentReference docRef = firestore.Collection("Leaderboards").Document(encryptDeviceID);
            docRef.SetAsync(new { Score = scoreCount, PlayerName = userName }).ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    onComplete?.Invoke(true);
                    Debug.Log("Test document written to Firebase Firestore");
                }
                else
                {
                    onComplete?.Invoke(false);
                    Debug.LogError("Failed to write test document");
                }
            });
        }
        catch (FirebaseException firebaseEx)
        {
            Debug.LogError("FirebaseException caught: " + firebaseEx.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("General Exception caught: " + ex.Message);
        }
    }

    public void FetchLeaderboardsFromServer(Action<bool> onComplete)
    {
        if (!canUseFirestore)
        {
            onComplete?.Invoke(false);  // Notify success
            return;
        }
        int myScore = 0;
        leaderboardsNameData.Clear();
        leaderboardsScoreData.Clear();
        CollectionReference collectionRef = firestore.Collection("Leaderboards");

        // Fetch documents in descending order by "Score"
        collectionRef.OrderByDescending("Score").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting documents: " + task.Exception);
                onComplete?.Invoke(false);
                return;
            }

            QuerySnapshot snapshot = task.Result;

            int loopCount = 0;
            // Loop through all documents in descending order of "Score"
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    loopCount++;

                    var data = document.ToDictionary();
                    string documentId = document.Id;
                    string playerName = data.ContainsKey("PlayerName") ? data["PlayerName"].ToString() : "Unknown";
                    myScore = data.ContainsKey("Score") ? int.Parse(data["Score"].ToString()) : 0;

                    // store top 9 data on device
                    if (loopCount < 10)
                    {
                        leaderboardsNameData.Add(playerName);
                        leaderboardsScoreData.Add(myScore);
                        // Display document ID and data
                        Debug.Log($"Rank: {loopCount} | PlayerName: {playerName} | Score: {myScore}");
                    }
                    // find out which one is our score for ranking the player 
                    if (documentId == encryptDeviceID) myRank = loopCount;
                }
                else
                {
                    Debug.Log("Document does not exist!");
                }
            }

            onComplete?.Invoke(true);
        });
    }

    
    private void InitializeAuthFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase initialized.");
#if UNITY_ANDROID
                SignInWithGoogle();
#elif UNITY_IPHONE
                PerformLoginWithAppleIdAndFirebase(user =>
                {
                    Debug.Log($"Apple ID username: {user.DisplayName}");
                });
#endif
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {task.Result}");
            }
        });
    }

    //------------------------GOOGLE SIGN IN------------------------
#if UNITY_ANDROID
    public void SignInWithGoogle()
    {
        Debug.Log("Google sign-in started");
        GoogleSignInConfiguration configuration = new GoogleSignInConfiguration
        {
            WebClientId = firebaseData.webSeceretID,
            RequestIdToken = true
        };

        GoogleSignIn.Configuration = configuration;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Google Sign-In failed: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                var googleUser = task.Result;
                var credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);

                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                {
                    if (authTask.IsFaulted)
                    {
                        Debug.LogError("Firebase Sign-In failed: " + authTask.Exception);
                        ActionManager.SignedInStatus?.Invoke(SignInType.GOOGLE, false);
                    }
                    else if (authTask.IsCompleted)
                    {
                        Debug.Log("Google Sign-In successful. User: " + auth.CurrentUser.DisplayName);
                        ActionManager.SignedInStatus?.Invoke(SignInType.GOOGLE, true);
                    }
                });
            }
            else
            {
                Debug.Log("Google Sign-In failed");
            }
        });
        Debug.Log("Google Sign-In Completed");
    }

#endif

//------------------------ iOS SIGN IN ------------------------
#if UNITY_IPHONE
    private static string GenerateRandomString(int length)
    {
        if (length <= 0)
        {
            throw new Exception("Expected nonce to have positive length");
        }

        const string charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvwxyz-._";
        var cryptographicallySecureRandomNumberGenerator = new RNGCryptoServiceProvider();
        var result = string.Empty;
        var remainingLength = length;

        var randomNumberHolder = new byte[1];
        while (remainingLength > 0)
        {
            var randomNumbers = new List<int>(16);
            for (var randomNumberCount = 0; randomNumberCount < 16; randomNumberCount++)
            {
                cryptographicallySecureRandomNumberGenerator.GetBytes(randomNumberHolder);
                randomNumbers.Add(randomNumberHolder[0]);
            }

            for (var randomNumberIndex = 0; randomNumberIndex < randomNumbers.Count; randomNumberIndex++)
            {
                if (remainingLength == 0)
                {
                    break;
                }

                var randomNumber = randomNumbers[randomNumberIndex];
                if (randomNumber < charset.Length)
                {
                    result += charset[randomNumber];
                    remainingLength--;
                }
            }
        }

        return result;
    }

    private static string GenerateSHA256NonceFromRawNonce(string rawNonce)
    {
        var sha = new SHA256Managed();
        var utf8RawNonce = Encoding.UTF8.GetBytes(rawNonce);
        var hash = sha.ComputeHash(utf8RawNonce);

        var result = string.Empty;
        for (var i = 0; i < hash.Length; i++)
        {
            result += hash[i].ToString("x2");
        }

        return result;
    }

    public void PerformLoginWithAppleIdAndFirebase(Action<FirebaseUser> firebaseAuthCallback)
    {
        var rawNonce = GenerateRandomString(32);
        var nonce = GenerateSHA256NonceFromRawNonce(rawNonce);

        var loginArgs = new AppleAuthLoginArgs(
            LoginOptions.IncludeEmail | LoginOptions.IncludeFullName,
            nonce);

        this.appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    this.PerformFirebaseAuthentication(appleIdCredential, rawNonce, firebaseAuthCallback);
                }
            },
            error =>
            {
                // Something went wrong
            });
    }

    private void PerformFirebaseAuthentication(
    IAppleIDCredential appleIdCredential,
    string rawNonce,
    Action<FirebaseUser> firebaseAuthCallback)
    {
        var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);
        var authorizationCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode);
        var firebaseCredential = OAuthProvider.GetCredential(
            "apple.com",
            identityToken,
            rawNonce,
            authorizationCode);

        this.auth.SignInWithCredentialAsync(firebaseCredential)
            .ContinueWithOnMainThread(task => HandleSignInWithUser(task, firebaseAuthCallback));
    }

    private static void HandleSignInWithUser(Task<FirebaseUser> task, Action<FirebaseUser> firebaseUserCallback)
    {
        if (task.IsCanceled)
        {
            Debug.Log("Firebase auth was canceled");
            firebaseUserCallback(null);
        }
        else if (task.IsFaulted)
        {
            Debug.Log("Firebase auth failed");
            firebaseUserCallback(null);
        }
        else
        {
            var firebaseUser = task.Result;
            Debug.Log("Firebase auth completed | User ID:" + firebaseUser.UserId);
            firebaseUserCallback(firebaseUser);
        }
    }
#endif

    //--------------------- PRIVATE FUNCTIONS ---------------------

    public List<string> GetLeaderboardsList()
    {
        return leaderboardsNameData;
    }

    public List<int> GetLeaderboardsScoreList()
    {
        return leaderboardsScoreData;
    }

    public int GetMyRank()
    {
        return myRank;
    }

    public bool GetConnectionStatus()
    {
        return canUseFirestore;
    }

}


public enum SignInType
{
    GOOGLE,
    APPLE,
    NONE
}
