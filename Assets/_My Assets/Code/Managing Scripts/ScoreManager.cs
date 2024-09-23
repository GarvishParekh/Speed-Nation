using Firebase.Auth;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    FirebaseInitlization firebaseScript;
    [Header("<size=15>SCRIPTS")]
    [SerializeField] private CarStatsManager carStatsManager;

    [Header("<size=15>USER INTERFACE")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text totalScoreText;
    [SerializeField] private TMP_Text resultScoreText;
    [SerializeField] private TMP_Text totalTimeSpentText;

    [Space]
    [SerializeField] private GameObject gameoverObject;
    [SerializeField] private GameObject highscoreObject;

    [Header("<size=15>VALUES")]
    [SerializeField] private float scoreCount;
    [SerializeField] private float totalScoreCount;
    [SerializeField] private float currentHighscoreCount;

    bool canCalculateScore = false;

    private void OnEnable()
    {
        GameoverController.Gameover += CalculateTotalScore;
        GameplayUiController.CountdownComplete += OnCompleteCountdown;
    }

    private void OnDisable()
    {
        GameoverController.Gameover -= CalculateTotalScore;
        GameplayUiController.CountdownComplete -= OnCompleteCountdown;
    }

    private void Start()
    {
        firebaseScript = FirebaseInitlization.instance;
        currentHighscoreCount = PlayerPrefs.GetInt(ConstantKeys.HIGHSCORE, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canCalculateScore) return;

        AddScore();
    }

    private void OnCompleteCountdown() => canCalculateScore = true;    

    private void AddScore()
    {
        scoreCount += Time.deltaTime * 50;
        scoreText.text = scoreCount.ToString("0");
        resultScoreText.text = "Score: " + scoreCount.ToString("0"); 
    }

    private void CalculateTotalScore()
    {
        float timeSpent = carStatsManager.GetTotalTimePlayed();

        totalTimeSpentText.text = "Time spent: " + timeSpent.ToString("0") + "s";
        totalScoreCount = scoreCount + timeSpent + carStatsManager.GetCarSmashedScore();
        totalScoreText.text = "Total: " + totalScoreCount.ToString("0");

        if (totalScoreCount > currentHighscoreCount)
        {
            PlayerPrefs.SetInt(ConstantKeys.HIGHSCORE, (int)totalScoreCount);
            firebaseScript.UpdateHighscoreOnServer(sucess =>
            {
                if (sucess)
                {
                    Debug.Log($"Highscore updated sucessfully");
                }
                else
                {
                    Debug.Log($"Error updating highscore");
                }
            });
            highscoreObject.gameObject.SetActive(true);
            gameoverObject.gameObject.SetActive(false);
        }
        else
        {
            highscoreObject.gameObject.SetActive(false);
            gameoverObject.gameObject.SetActive(true);
        }
    }
}
