using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreCount;
    
    // Update is called once per frame
    void Update()
    {
        AddScore();
    }

    private void AddScore()
    {
        scoreCount += Time.deltaTime * 50;
        scoreText.text = scoreCount.ToString("0"); 
    }
}
