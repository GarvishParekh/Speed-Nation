using UnityEngine;

[CreateAssetMenu(fileName = "Economy Data", menuName = "Scriptable/Economy Data")]
public class EconomyData : ScriptableObject
{
    public int totalOil = 0;
    public int startingOilValue = 75;

    public int totalTickets = 0;
    public int startingTicketValue = 75;
}
