using UnityEngine;
using UnityEngine.SceneManagement;

public class BgMusicManager : MonoBehaviour
{
    public static BgMusicManager instance;
    [SerializeField] private GameObject bgMusic;
    [SerializeField] private GameObject bgMusicMainMenu;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
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
            bgMusicMainMenu.SetActive(true);   
        }
    }

    public void StartMusic()
    {
        bgMusic.SetActive(true);
        bgMusicMainMenu.SetActive(false);   
    }
}
