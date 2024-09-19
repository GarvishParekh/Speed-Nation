using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class UpdateLeaderBoards : MonoBehaviour
{
    FirebaseInitlization firebaseScript;
    [SerializeField] private List<LeaderboardCellIdentity> cellIdentity = new List<LeaderboardCellIdentity>();

    private void Start()
    {
        firebaseScript = FirebaseInitlization.instance;
    }

    // Update is called once per frame
    public void UpdateUI()
    {
        firebaseScript.UpdateHighscoreOnServer();
        firebaseScript.FetchLeaderboardsFromServer();

        Invoke(nameof(Delay), 1);
    }

    private void Delay()
    {
        List<string> lbList = firebaseScript.GetLeaderboardsList();
        int index = 0;
        foreach (LeaderboardCellIdentity cell in cellIdentity)
        {
            if (index < lbList.Count)
            {
                string[] parts = lbList[index].Split("_");
                cell.SetNameAndRank(parts[0], int.Parse(parts[1]));
            }
            else
            {
                cell.SetNameAndRank("-", 0);
            }

            index++;
        }
    }
}
