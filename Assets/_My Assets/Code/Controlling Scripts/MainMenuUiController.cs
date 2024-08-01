using UnityEngine;

public class MainMenuUiController : MonoBehaviour
{
    UiManager uiManager;

    private void Start()
    {
        uiManager = UiManager.instance;
        uiManager.StartCanvasRoutine(CanvasNames.MAIN_MENU);
    }

    public void _DriveButton()
    {
        uiManager.StartSceneChangeRoutine(ConstantKeys.SCENE_GAMEPLAY);
    }
    
}
