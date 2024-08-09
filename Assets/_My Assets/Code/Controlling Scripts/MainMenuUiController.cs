using UnityEngine;

public class MainMenuUiController : MonoBehaviour
{
    UiManager uiManager;
    BgMusicManager bgMusicManager;

    private void Start()
    {
        uiManager = UiManager.instance;
        bgMusicManager = BgMusicManager.instance;

        uiManager.StartCanvasRoutine(CanvasNames.MAIN_MENU);
    }

    public void _DriveButton()
    {
        bgMusicManager.StartMusic();
        uiManager.StartSceneChangeRoutine(ConstantKeys.SCENE_GAMEPLAY);
    }
    public void _PracticeButton()
    {
        uiManager.StartSceneChangeRoutine(ConstantKeys.SCENE_PRACTICE);
    }

    public void _GrageButton()
    {
        uiManager.StartCanvasRoutine(CanvasNames.GARAGE);
    }

    public void _MainMenuButton()
    {
        uiManager.StartCanvasRoutine(CanvasNames.MAIN_MENU);
    }
}
