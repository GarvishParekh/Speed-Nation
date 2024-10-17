using TMPro;
using UnityEngine;

public class LeaderboardCellIdentity : MonoBehaviour
{
    [SerializeField] private int cellIndex = 0;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        rankText.text = "0" + transform.GetSiblingIndex().ToString();
    }

    public void SetNameAndRank(string playerName, int score)
    {
        nameText.text = playerName;
        if (score == 0)
        {
            scoreText.text = "-";
        }
        else scoreText.text = score.ToString();
    }
}
