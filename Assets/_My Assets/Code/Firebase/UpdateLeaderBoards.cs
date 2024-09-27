using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class UpdateLeaderBoards : MonoBehaviour
{
    FirebaseInitlization firebaseScript;
    [SerializeField] private List<LeaderboardCellIdentity> cellIdentity = new List<LeaderboardCellIdentity>();
    
    [SerializeField] private TMP_Text myRankText;

    [SerializeField] private GameObject fetchingYourData;
    [SerializeField] private GameObject somethingWentWrong;
    [SerializeField] private GameObject leaderboardData;

    private void Start()
    {
        firebaseScript = FirebaseInitlization.instance;
    }

    // Update is called once per frame
    public void UpdateUI()
    {
        DisplayStatus(fetchingYourData);

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
        firebaseScript.FetchLeaderboardsFromServer(sucess =>
        {
        if (sucess)
        {
            DisplayStatus(leaderboardData);

            List<string> lbNameList = firebaseScript.GetLeaderboardsList();
            List<int> lbScoreList = firebaseScript.GetLeaderboardsScoreList();
            int index = 0;
            foreach (LeaderboardCellIdentity cell in cellIdentity)
            {
                if (index < lbNameList.Count)
                {
                        cell.SetNameAndRank(lbNameList[index], lbScoreList[index]);
                }
                else
                {
                    cell.SetNameAndRank("-", 0);
                }

                    index++;
                }

                myRankText.text = $"YOUR RANK IS {firebaseScript.GetMyRank().ToString()}th";
            }
            else
            {
                DisplayStatus(somethingWentWrong);
                return;
            }
        });

    }

    private void DisplayStatus(GameObject panelToShow)
    {
        fetchingYourData.SetActive(false);
        somethingWentWrong.SetActive(false);
        leaderboardData.SetActive(false);

        panelToShow.SetActive(true);   
    }
}
