using UnityEngine;

public class MainMenuUiController : MonoBehaviour
{
    UiManager uiManager;
    BgMusicManager bgMusicManager;

    private void Start()
    {
        uiManager = UiManager.instance;
        bgMusicManager = BgMusicManager.instance;

        uiManager.OpenCanvasWithShutter(CanvasNames.MAIN_MENU);
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
        uiManager.OpenCanvasWithShutter(CanvasNames.GARAGE);
    }

    public void _MainMenuButton()
    {
        uiManager.OpenCanvasWithShutter(CanvasNames.MAIN_MENU);
    }
}
