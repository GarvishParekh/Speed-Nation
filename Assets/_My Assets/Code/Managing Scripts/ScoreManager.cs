using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("<size=15>SCRIPTS")]
    [SerializeField] private CarStatsManager carStatsManager;

    [Header("<size=15>USER INTERFACE")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text totalScoreText;
    [SerializeField] private TMP_Text resultScoreText;
    [SerializeField] private TMP_Text newHighscoreText;

    [Header("<size=15>VALUES")]
    [SerializeField] private float scoreCount;
    [SerializeField] private float totalScoreCount;
    [SerializeField] private float currentHighscoreCount;

    private void OnEnable()
    {
        GameoverController.Gameover += CalculateTotalScore;
    }

    private void OnDisable()
    {
        GameoverController.Gameover -= CalculateTotalScore;
    }

    private void Start()
    {
        currentHighscoreCount = PlayerPrefs.GetInt(ConstantKeys.HIGHSCORE, 0);
    }

    // Update is called once per frame
    void Update()
    {
        AddScore();
    }

    private void AddScore()
    {
        scoreCount += Time.deltaTime * 50;
        scoreText.text = scoreCount.ToString("0");
        resultScoreText.text = "Score: " + scoreCount.ToString("0"); 
    }

    private void CalculateTotalScore()
    {
        totalScoreCount = scoreCount + carStatsManager.GetFuelScore() + carStatsManager.GetCarSmashedScore();
        totalScoreText.text = "Total: " + totalScoreCount.ToString("0");

        if (totalScoreCount > currentHighscoreCount)
        {
            PlayerPrefs.SetInt(ConstantKeys.HIGHSCORE, (int)totalScoreCount);
            newHighscoreText.gameObject.SetActive(true);
        }
        else newHighscoreText.gameObject.SetActive(false);
    }
}
