using UnityEngine;

public enum CarsName
{
    MIDNIGHT_SPRINT_S,
    CRIMSON_DRIFT_S,
    URBAN_RIDE_S,
    TUNDER_DRIFT_SS,
    BUBBEGUM_PUP_H,
    VERDANT_VELOCITY_H,
    SUMMER_DREAMER_M,
    SPECTRUM_FLASH_M,
    GOLDEN_STREAK_E,
    TRAILBLAZER_SHADOW_E,
    SCARLET_VELOCE_E,
}

public enum LockStatus
{
    LOCKED,
    UNLOCKED
}

public enum PurchaseWay
{
    FREE,
    TICKETS,
    IN_APP
}

[CreateAssetMenu(fileName = "CarDetailsData", menuName = "Scriptable / CarDetailsData")]
public class CarDetailsData : ScriptableObject
{
    public CarDetail[] carDetail;
}

[System.Serializable]
public class CarDetail
{
    public string carName;
    public int carIndex;
    public int requriedScore = 2000;
    public int requriedTickets = 500;

    [Space]
    public Texture selectedSprite;
    public Texture unSelectedSprite;

    [Space]
    public PurchaseWay purchaseWay;
    public LockStatus lockStatus;
    public bool isSelected = false;
}
