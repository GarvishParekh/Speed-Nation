using TMPro;
using Firebase;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // singleton
    FirebaseInitlization firebaseScript;

    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private GameplayData gameplayData;
    [SerializeField] private CarDetailsData carDetailData;
    [SerializeField] private EconomyData economyData;

    [Header("<size=15>SCRIPTS")]
    [SerializeField] private CarStatsManager carStatsManager;

    [Header("<size=15>USER INTERFACE")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text totalScoreText;
    [SerializeField] private TMP_Text userNameText;
    [SerializeField] private TMP_Text carNameText;
    [SerializeField] private TMP_Text gainedTicketText;
    [SerializeField] private TMP_Text gaindOilText;

    [Space]
    [SerializeField] private GameObject gameoverObject;
    [SerializeField] private GameObject highscoreObject;

    bool canCalculateScore = false;

    private void OnEnable()
    {
        GameoverController.Gameover += CalculateTotalScore;
        GameoverController.Gameover += CalculateGainedEconomy;
        ActionManager.GameoverRewardAdsWatched += RewardAdsOil;

        ActionManager.countDownCompleted += OnCompleteCountdown;
        ActionManager.CarCollided += OnCarCollision;
        ActionManager.PlayerBoosting += OnBoosting;
    }

    private void OnDisable()
    {
        GameoverController.Gameover -= CalculateTotalScore;
        GameoverController.Gameover -= CalculateGainedEconomy;
        ActionManager.GameoverRewardAdsWatched -= RewardAdsOil;

        ActionManager.countDownCompleted -= OnCompleteCountdown;
        ActionManager.CarCollided -= OnCarCollision;
        ActionManager.PlayerBoosting -= OnBoosting;
    }

    private void OnBoosting(bool check)
    {
        if (check) gameplayData.scoreMultiplyer = gameplayData.boostedScoreMultiplyer;
        else gameplayData.scoreMultiplyer = gameplayData.normalScoreMultiplyer;
    }

    private string FetchCarName()
    {
        int selectedCarIndex = PlayerPrefs.GetInt(ConstantKeys.SELECTED_CAR, 0);
        string carNameString = carDetailData.carDetail[selectedCarIndex].carName;
        return carNameString;
    }

    private void Start()
    {
        carNameText.text = $"<SIZE=20>TRAVLING IN</SIZE> {FetchCarName()}";
        userNameText.text = PlayerPrefs.GetString(ConstantKeys.USERNAME);

        ResetScoreData();
        firebaseScript = FirebaseInitlization.instance;
        gameplayData.currentHighscoreCount = PlayerPrefs.GetInt(ConstantKeys.HIGHSCORE, 0);
    }

    private void ResetScoreData()
    {
        // reset score in scriptable
        gameplayData.scoreCount = 0;
        gameplayData.totalScoreCount = 0;
        gameplayData.scoreMultiplyer = gameplayData.normalScoreMultiplyer;

        // reset economy gained per round
        economyData.gainedOilsPerRound = 0;
        economyData.gainedTicketsPerRound = 0;
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
        gameplayData.scoreCount += Time.deltaTime * gameplayData.scoreMultiplyer;
        scoreText.text = gameplayData.scoreCount.ToString("0");
    }

    public void AddScore(int scoreToAdd)
    {
        gameplayData.scoreCount += scoreToAdd;
        scoreText.text = gameplayData.scoreCount.ToString("0");
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

        gameplayData.totalScoreCount = gameplayData.scoreCount + timeSpent + carStatsManager.GetCarSmashedScore();
        totalScoreText.text = "<SIZE=30>SCORE</SIZE> " + gameplayData.totalScoreCount.ToString("0");

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

    private void CalculateGainedEconomy()
    {
        int totalTickets = PlayerPrefs.GetInt(ConstantKeys.TOTAL_TICKETS);
        int totalOil = PlayerPrefs.GetInt(ConstantKeys.TOTAL_OIL); 

        economyData.gainedOilsPerRound = (int)gameplayData.totalScoreCount / 6;
        economyData.gainedTicketsPerRound = ((int)gameplayData.totalScoreCount / 10000) + GetRandomGrance();

        totalOil += economyData.gainedOilsPerRound;
        totalTickets += economyData.gainedTicketsPerRound;

        PlayerPrefs.SetInt(ConstantKeys.TOTAL_OIL, totalOil);
        PlayerPrefs.SetInt(ConstantKeys.TOTAL_TICKETS, totalTickets);

        UpdateGameoverEconomyUI();
    }
   
    private void RewardAdsOil()
    {
        int totalOil = PlayerPrefs.GetInt(ConstantKeys.TOTAL_OIL);
        totalOil += 750;
        PlayerPrefs.SetInt(ConstantKeys.TOTAL_OIL, totalOil);

        UpdateGameoverEconomyUI();
    }

    private void UpdateGameoverEconomyUI()
    {
        gaindOilText.text = economyData.gainedOilsPerRound.ToString();
        gainedTicketText.text = economyData.gainedTicketsPerRound.ToString();
    }

    private int GetRandomGrance()
    {
        int randomGrace = 0;

        if ((int)gameplayData.totalScoreCount > 5000) randomGrace = Random.Range(0, 4);
        return randomGrace;
    }
}
