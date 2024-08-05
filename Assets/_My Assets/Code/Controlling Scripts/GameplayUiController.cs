using System.Security.Cryptography;
using UnityEngine;

public class GameplayUiController : MonoBehaviour
{
    UiManager uiManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] GameObject controlCanvas;

    private void Start()
    {
        inputManager.enabled = false;

        uiManager = UiManager.instance;
        uiManager.OpenShutter();

        Invoke(nameof(EnableControls), 9);
    }

    private void EnableControls()
    {
        inputManager.enabled = true;    
    }

    public void _QuitButton()
    {
        uiManager.StartSceneChangeRoutine(ConstantKeys.SCENE_MAIN_MENU);
    }
}
