using UnityEngine;

public class GameplayUiController : MonoBehaviour
{
    UiManager uiManager;
    [SerializeField] GameObject controlCanvas;

    private void Start()
    {
        controlCanvas.SetActive(false);
        uiManager = UiManager.instance;
        uiManager.OpenShutter();
    }

    public void _QuitButton()
    {
        uiManager.StartSceneChangeRoutine(ConstantKeys.SCENE_MAIN_MENU);
    }

    public void EnableControls()
    {
        controlCanvas.SetActive(true);
    }
}
