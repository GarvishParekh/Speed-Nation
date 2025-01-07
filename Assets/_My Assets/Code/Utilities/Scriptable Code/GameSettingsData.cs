using UnityEngine;

[CreateAssetMenu(fileName = "Settings Data", menuName = "Scriptable/Game Settings Data")]
public class GameSettingsData : ScriptableObject
{
    public PostProcessingStatus postProcessingStatus;
    public MusicStatus musicStatus;
    public SoundEffectsStatus soundEffectsStatus;
    public int totalTimeSpent = 0;
}

public enum PostProcessingStatus
{
    HIGH,
    LOW
}

public enum MusicStatus
{
    ON,
    OFF
}

public enum SoundEffectsStatus
{
    ON,
    OFF
}
