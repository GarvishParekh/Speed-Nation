using TMPro;
using UnityEngine;
using System.Collections.Generic;


public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;  

    UiManager uiManager;
    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private EconomyData economyData;

    [Header("<size=15>UI")]
    [SerializeField] private List<TMP_Text> ticketCountText;

    private void Awake()
    {
        instance = this;    
        economyData.totalTickets = FetchTicketAmount();
        UpdateTicketCountUi(economyData.totalTickets);
    }

    private void Start()
    {
        uiManager = UiManager.instance;
    }

    private int FetchTicketAmount()
    {
        return PlayerPrefs.GetInt(ConstantKeys.TOTAL_TICKETS, economyData.startingValue);
    }

    private void SaveBalance()
    {
        PlayerPrefs.SetInt(ConstantKeys.TOTAL_TICKETS, economyData.totalTickets);
    }

    // updating all the ui text in main menu scene
    private void UpdateTicketCountUi(int ticketAmount)
    {
        foreach (TMP_Text ticketText in ticketCountText)
        {
            ticketText.text = ticketAmount.ToString();
        }
    }

    public void CreditBalance (int amountToAdd)
    {
        economyData.totalTickets += amountToAdd;
        UpdateTicketCountUi(economyData.totalTickets);

        SaveBalance();
    }

    public void DebitBalance (int amountToSub)
    {
        economyData.totalTickets -= amountToSub;
        UpdateTicketCountUi(economyData.totalTickets);

        SaveBalance();
    }

    public bool CheckBalance(int amountToCheck)
    {
        if (amountToCheck > economyData.totalTickets)
        {
            uiManager.OpenPopUp(CanvasNames.OUT_OF_TICKETS_CANVAS);
            return false;
        }
        else return true;
    }
}
