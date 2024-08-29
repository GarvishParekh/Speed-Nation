using UnityEngine;

[CreateAssetMenu(fileName = "CarDetailsData", menuName = "Scriptable / CarDetailsData")]
public class CarDetailsData : ScriptableObject
{
    public CarDetail[] carDetail;
    public Sprite selectedCarSprite;
    public Sprite unSelectedCarSprite;
    public Color selectedColor;
    public Color unSelectedColor;
}

[System.Serializable]
public class CarDetail
{
    public string carName;
    public int carIndex;
    public string carDescription;
    public Sprite carIconSprite;
    public LockStatus lockStatus;
    public bool isSelected = false;
}
