using UnityEngine;

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

    [Space]
    public Sprite selectedSprite;
    public Sprite unSelectedSprite;

    [Space]
    public LockStatus lockStatus;
    public bool isSelected = false;
}
