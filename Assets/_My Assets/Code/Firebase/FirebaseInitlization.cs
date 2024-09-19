using Firebase;
using UnityEngine;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Crashlytics;
using Firebase.Firestore;
using System.Collections.Generic;

public class FirebaseInitlization : MonoBehaviour
{
    public static FirebaseInitlization instance;
    [SerializeField] private List<string> leaderboardData;


    private void Awake()
    {
        CreateSingleton();
        FirestoreInitializer();
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
        FirebaseAnalyticsInitilization();
    }

    private void FirebaseAnalyticsInitilization()
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
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    private FirebaseFirestore firestore;
    private void FirestoreInitializer()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;

            // Initialize Firebase Firestore
            InitializeFirestore();
        });
    }

    void InitializeFirestore()
    {
        firestore = FirebaseFirestore.DefaultInstance;
        Debug.Log("Firebase Firestore initialized");
        //UpdateHighscoreOnServer();
        //FetchLeaderboardsFromServer();
    }

    string userName = "Garvish";
    int scoreCount = 500;
    public void UpdateHighscoreOnServer()
    {
        userName = PlayerPrefs.GetString(ConstantKeys.USERNAME);
        scoreCount = PlayerPrefs.GetInt(ConstantKeys.HIGHSCORE, 0);

        DocumentReference docRef = firestore.Collection("Leaderboards").Document(SystemInfo.deviceUniqueIdentifier);
        docRef.SetAsync(new { Score = scoreCount, PlayerName = userName}).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Test document written to Firebase Firestore");
            }
            else
            {
                Debug.LogError("Failed to write test document");
            }
        });
    }

    public void FetchLeaderboardsFromServer()
    {
        leaderboardData.Clear();
        CollectionReference collectionRef = firestore.Collection("Leaderboards");

        // Fetch documents in descending order by "Score"
        collectionRef.OrderByDescending("Score").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting documents: " + task.Exception);
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
                    int score = data.ContainsKey("Score") ? int.Parse(data["Score"].ToString()) : 0;

                    leaderboardData.Add($"{playerName}_{score}");

                    // Display document ID and data
                    Debug.Log($"Rank: {loopCount} | PlayerName: {playerName} | Score: {score}");
                }
                else
                {
                    Debug.Log("Document does not exist!");
                }
            }
        });
    }

    public List<string> GetLeaderboardsList()
    {
        return leaderboardData;
    }
}
