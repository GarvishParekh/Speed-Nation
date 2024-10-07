using TMPro;
using Firebase;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // singleton
    FirebaseInitlization firebaseScript;

    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private GameplayData gameplayData;
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

    bool canCalculateScore = false;

    private void OnEnable()
    {
        GameoverController.Gameover += CalculateTotalScore;
        ActionManager.countDownCompleted += OnCompleteCountdown;
        ActionManager.CarCollided += OnCarCollision;
    }

    private void OnDisable()
    {
        GameoverController.Gameover -= CalculateTotalScore;
        ActionManager.countDownCompleted -= OnCompleteCountdown;
        ActionManager.CarCollided -= OnCarCollision;
    }

    private void Start()
    {
        ResetScoreData();
        firebaseScript = FirebaseInitlization.instance;
        gameplayData.currentHighscoreCount = PlayerPrefs.GetInt(ConstantKeys.HIGHSCORE, 0);
    }

    private void ResetScoreData()
    {
        // reset score in scriptable
        gameplayData.scoreCount = 0;
        gameplayData.totalScoreCount = 0;
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
        gameplayData.scoreCount += Time.deltaTime * 50;
        scoreText.text = gameplayData.scoreCount.ToString("0");
        resultScoreText.text = "Score: " + gameplayData.scoreCount.ToString("0"); 
    }

    public void AddScore(int scoreToAdd)
    {
        gameplayData.scoreCount += scoreToAdd;
        scoreText.text = gameplayData.scoreCount.ToString("0");
        resultScoreText.text = "Score: " + gameplayData.scoreCount.ToString("0");
    }

    private void OnCarCollision(Transform t)
    {
        if (gameplayData.isBoosting)
        {
            AddScore(500);
            ActionManager.TrafficKilled?.Invoke();
        }
    }

    private void CalculateTotalScore()
    {
        float timeSpent = carStatsManager.GetTotalTimePlayed();

        totalTimeSpentText.text = "Time spent: " + timeSpent.ToString("0") + "s";
        gameplayData.totalScoreCount = gameplayData.scoreCount + timeSpent + carStatsManager.GetCarSmashedScore();
        totalScoreText.text = "Total: " + gameplayData.totalScoreCount.ToString("0");

        if (gameplayData.totalScoreCount > gameplayData.currentHighscoreCount)
        {
            highscoreObject.gameObject.SetActive(true);
            gameoverObject.gameObject.SetActive(false);

            PlayerPrefs.SetInt(ConstantKeys.HIGHSCORE, (int)gameplayData.totalScoreCount);

            try
            {
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
        else
        {
            highscoreObject.gameObject.SetActive(false);
            gameoverObject.gameObject.SetActive(true);
        }
    }
}
