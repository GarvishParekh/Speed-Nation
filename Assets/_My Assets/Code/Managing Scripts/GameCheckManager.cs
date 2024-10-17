using UnityEngine;

public class GameCheckManager : MonoBehaviour
{
    public static GameCheckManager instance;

    bool newsSeen = false;
    private void Awake()
    {
        CreateSingleton();
    }
    

    private void CreateSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool GetNewsSeen()
    {
        return newsSeen;
    }

    public void SetNewsSeen(bool check)
    {
        newsSeen = check;
    }
}
