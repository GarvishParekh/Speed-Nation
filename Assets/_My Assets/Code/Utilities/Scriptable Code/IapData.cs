using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IAP Data", menuName = "Scriptable/IAP Data")]
public class IapData : ScriptableObject
{
    public ConsumableItem basicTicketCard;
    public ConsumableItem valueTicketCard;
    public ConsumableItem premiumTicketCard;
    public ConsumableItem bestDealTicketCard;

    public SubscriptionItem adsSubscriptionItem;
}

[Serializable]
public class ConsumableItem
{
    public string Name;
    public string Id;
    public string Description;
    public string Price;
}


[Serializable]
public class NonConsumableItem
{
    public string Name;
    public string Id;
    public string Description;
    public string Price;
}

[Serializable]
public class SubscriptionItem
{
    public string Name;
    public string Id;
    public string Description;
    public string Price;
    public int timeDuration;
}
