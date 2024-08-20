using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("<size=15>SCRIPTS")]
    [SerializeField] private CarStatsManager carStatsManager;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text totalScoreText;
    [SerializeField] private TMP_Text resultScoreText;
    [SerializeField] private float scoreCount;
    [SerializeField] private float totalScoreCount;

    private void OnEnable()
    {
        GameoverController.Gameover += CalculateTotalScore;
    }

    private void OnDisable()
    {
        GameoverController.Gameover -= CalculateTotalScore;
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
    }
}
