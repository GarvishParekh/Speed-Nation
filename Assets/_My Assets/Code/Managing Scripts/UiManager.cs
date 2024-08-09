using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [SerializeField] private List<CanvasIdentity> uiCanvas;
    [SerializeField] private GameObject shutter;

    [Header ("<size=15>CAMERAS")]
    [SerializeField] private GameObject mainMenuCam;
    [SerializeField] private GameObject garageCam;

    WaitForSeconds shutterTime = new WaitForSeconds(1f);

    private void Awake()
    {
        instance = this;
    }

    public void StartCanvasRoutine(CanvasNames desireCanvas)
    {
        Debug.Log("Start canvas routine");
        StartCoroutine(nameof(OpenUICanvas), desireCanvas);
    }

    private IEnumerator OpenUICanvas(CanvasNames desireName)
    {
        Debug.Log("Routine started");
        CloseShutter();
        yield return shutterTime;
        foreach (CanvasIdentity canvasInfo in uiCanvas)
        {
            if (canvasInfo.GetCanvasName() == desireName)
            {
                canvasInfo.GetComponent<ICanvasController>().EnableCanvas();
            }
            else
            {
                canvasInfo.GetComponent<ICanvasController>().DisableCanvas();
            }
        }

        switch (desireName)
        {
            case CanvasNames.MAIN_MENU:
                mainMenuCam.SetActive(true);
                garageCam.SetActive(false);
                break;
            case CanvasNames.GARAGE:
                mainMenuCam.SetActive(false);
                garageCam.SetActive(true);
                break;
        }
        OpenShutter();
        yield return null;      
    }

    public void StartSceneChangeRoutine(string sceneName)
    {
        StartCoroutine(nameof(ChangeScene), sceneName);
    }

    private IEnumerator ChangeScene(string sceneName)
    {
        Debug.Log("Routine started");
        CloseShutter();
        yield return shutterTime;
        
        SceneManager.LoadScene(sceneName);
    }


    public void CloseShutter()
    {
        LeanTween.moveLocal(shutter, Vector3.zero, 0.5f);
    }

    public void OpenShutter()
    {
        LeanTween.moveLocal(shutter, new Vector3(-3000f, 0, 0), 0.5f);
    }
}
