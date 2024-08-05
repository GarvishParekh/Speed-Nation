using UnityEngine;
using UnityEngine.SceneManagement;

public class BgMusicManager : MonoBehaviour
{
    public static BgMusicManager instance;
    [SerializeField] private GameObject bgMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);  
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == ConstantKeys.SCENE_MAIN_MENU)
        {
            bgMusic.SetActive(false);   
        }
    }

    public void StartMusic()
    {
        bgMusic.SetActive(true);   
    }
}
