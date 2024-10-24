using TMPro;
using UnityEngine;
using System.Collections.Generic;


public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;  
    AdsManager adsManager;  

    UiManager uiManager;
    [Header("<size=15>SCRIPTABLE")]
    [SerializeField] private EconomyData economyData;

    [Header("<size=15>UI")]
    [SerializeField] private List<TMP_Text> ticketCountText;
    [SerializeField] private List<TMP_Text> oilCountText;

    private void OnEnable()
    {
        ActionManager.rewardOils += CreditOil;
        ActionManager.rewardTickets += CreditTickets;
    }

    private void OnDisable()
    {
        ActionManager.rewardOils -= CreditOil;
        ActionManager.rewardTickets -= CreditTickets;
    }

    private void Awake()
    {
        instance = this;    
        economyData.totalTickets = FetchTicketBalance();
        economyData.totalOil = FetchOilBalance();
        UpateEconomyUI(economyData.totalTickets, economyData.totalOil);
    }

    private void Start()
    {
        uiManager = UiManager.instance;
        adsManager = AdsManager.instance;   
    }

    private int FetchTicketBalance()
    {
        return PlayerPrefs.GetInt(ConstantKeys.TOTAL_TICKETS, economyData.startingTicketValue);
    }
    private int FetchOilBalance()
    {
        return PlayerPrefs.GetInt(ConstantKeys.TOTAL_OIL, economyData.startingOilValue);
    }


    private void SaveEconomy()
    {
        PlayerPrefs.SetInt(ConstantKeys.TOTAL_TICKETS, economyData.totalTickets);
        PlayerPrefs.SetInt(ConstantKeys.TOTAL_OIL, economyData.totalOil);
    }

    // updating all the ui text in main menu scene
    private void UpateEconomyUI(int ticketAmount, int oilAmount)
    {
        foreach (TMP_Text ticketText in ticketCountText)
        {
            ticketText.text = ticketAmount.ToString();
        }

        foreach (TMP_Text oilText in oilCountText)
        {
            oilText.text = oilAmount.ToString();
        }
    }

    public void CreditTickets (int amountToAdd)
    {
        economyData.totalTickets += amountToAdd;
        UpateEconomyUI(economyData.totalTickets, economyData.totalOil);

        SaveEconomy();
    }

    public void DebitTickets (int amountToSub)
    {
        economyData.totalTickets -= amountToSub;
        UpateEconomyUI(economyData.totalTickets, economyData.totalOil);

        SaveEconomy();
    }

    public void CreditOil(int amountToAdd)
    {
        economyData.totalOil += amountToAdd;
        UpateEconomyUI(economyData.totalTickets, economyData.totalOil);

        SaveEconomy();
    }

    public void DebitOil(int amountToSub)
    {
        economyData.totalOil -= amountToSub;
        UpateEconomyUI(economyData.totalTickets, economyData.totalOil);

        SaveEconomy();
    }

    public bool CheckTicketBalance(int amountToCheck)
    {
        if (amountToCheck > economyData.totalTickets)
        {
            uiManager.OpenPopUp(CanvasNames.OUT_OF_TICKETS_CANVAS);
            return false;
        }
        else return true;
    }

    public bool CheckOilBalance(int amountToCheck)
    {
        if (amountToCheck > economyData.totalOil)
        {
            uiManager.OpenPopUp(CanvasNames.OUT_OF_TICKETS_CANVAS);
            return false;
        }
        else return true;
    }

    public void _BasicCardPurchase()
    {
        if (CheckTicketBalance(50))
        {
            DebitTickets(50);
            CreditOil(10000);
        }
    }

    public void _ValueCardPurchase()
    {
        if (CheckTicketBalance(50))
        {
            DebitTickets(50);
            CreditOil(10000);
        }
    }

    public void _EpicCardPurchase()
    {
        if (CheckTicketBalance(50))
        {
            DebitTickets(50);
            CreditOil(10000);
        }
    }

    public void _OilRewardCard()
    {
        adsManager.ShowRewardedAds(RewardType.OILS);
    }

    public void _TicketRewardCard()
    {
        adsManager.ShowRewardedAds(RewardType.TICKETS);
    }
}
