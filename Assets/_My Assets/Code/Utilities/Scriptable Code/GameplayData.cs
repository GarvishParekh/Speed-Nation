using UnityEngine;

[CreateAssetMenu(fileName = "Gameplay Data", menuName = "Scriptable / GameplayData")]
public class GameplayData : ScriptableObject
{
    [Header ("<size=15>BOOST SETTINGS")]
    public bool isBoosting = false;
    public float boostTimer = 1;
    public int addingBoostValue = 2;

    [Header ("<size=15>SCORE SETTINGS")]
    public float scoreCount;
    public float totalScoreCount;
    public float currentHighscoreCount;

    [Header ("<size=15>BOOST SCORE VALUE")]
    public int singeKillValue;
    public int doubleKillValue;
    public int tripleKillValue;
    public int quadKillValue;
    public int pentaCrushValue;
    public int rampageValue;
    public int godlikeValue;
    public int unstopableValue;
}
