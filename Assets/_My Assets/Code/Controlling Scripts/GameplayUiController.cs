using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayUiController : MonoBehaviour
{
    UiManager uiManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject controlCanvas;

    [SerializeField] private GameObject threeObject;
    [SerializeField] private GameObject twoObject;
    [SerializeField] private GameObject oneObject;
    [SerializeField] private GameObject goObject;
    [SerializeField] private List<CanvasGroup> plus500 = new List<CanvasGroup>();

    [Space]
    [SerializeField] private CanvasGroup onBoostComponents;

    [Space]
    [SerializeField] private ParticleSystem speedingLines;

    private void OnEnable()
    {
        ActionManager.PlayerBoosting += OnPlayerBoost;
        ActionManager.CarCollided += OnCarCollision;
    }

    private void OnDisable()
    {
        ActionManager.PlayerBoosting -= OnPlayerBoost;
        ActionManager.CarCollided -= OnCarCollision;
    }

    private void Start()
    {
        inputManager.enabled = false;

        uiManager = UiManager.instance;
        //uiManager.OpenShutter();
        uiManager.OpenCanvasWithShutter(CanvasNames.GAMEPLAY);

        StartCoroutine(nameof(StartCountDown));
    }

    private void EnableControls()
    {
        inputManager.enabled = true;    
    }

    public void _QuitButton()
    {
        //Time.timeScale = 1;
        uiManager.StartSceneChangeRoutine(ConstantKeys.SCENE_MAIN_MENU);
    }

    public void _PuaseButton()
    {
        uiManager.OpenCanvasWithoutShutter(CanvasNames.PAUSE);   
        Time.timeScale = 0;
    }

    public void _ResumeButton()
    {
        Time.timeScale = 1;
        uiManager.OpenCanvasWithoutShutter(CanvasNames.GAMEPLAY);
    }

    public void _RestartButton()
    {
        uiManager.StartSceneChangeRoutine(ConstantKeys.SCENE_GAMEPLAY);
    }

    private IEnumerator StartCountDown()
    {
        threeObject.transform.localScale = Vector3.zero;
        twoObject.transform.localScale = Vector3.zero;
        oneObject.transform.localScale = Vector3.zero;
        goObject.transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(2);

        LeanTween.scale(threeObject, Vector3.one, 1.0f).setEaseInOutSine();
        LeanTween.rotateZ(threeObject, 2160, 1.0f).setEaseInOutSine().setOnComplete(()=>
        {
            LeanTween.scale(threeObject, Vector3.zero, 0.15f).setEaseInOutSine().setDelay(0.3f);
        });

        yield return new WaitForSeconds(1.5f);

        LeanTween.scale(twoObject, Vector3.one, 1.0f).setEaseInOutSine().setOnComplete(() =>
        {
            LeanTween.scale(twoObject, Vector3.zero, 0.15f).setEaseInOutSine().setDelay(0.3f);
        });

        yield return new WaitForSeconds(1.5f);

        LeanTween.scale(oneObject, Vector3.one, 1.0f).setEaseInOutSine().setOnComplete(() =>
        {
            LeanTween.scale(oneObject, Vector3.zero, 0.15f).setEaseInOutSine().setDelay(0.3f);
        });

        yield return new WaitForSeconds(1.5f);

        LeanTween.scale(goObject, Vector3.one, 1.0f).setEaseInOutSine().setOnComplete(() =>
        {
            LeanTween.alphaCanvas(goObject.GetComponent<CanvasGroup>(), 0, 0.35f).setEaseInOutSine().setDelay(0.3f).setOnComplete(() =>
            {
                ActionManager.countDownCompleted?.Invoke();
                EnableControls();
            }); ;
            LeanTween.scale(goObject, Vector3.one * 20, 2f).setEaseInOutSine().setDelay(0.3f);
        });
    }

    bool isBoosting = false;
    private void OnPlayerBoost(bool check)
    {
        isBoosting = check; 
        if (check)
        {
            LeanTween.alphaCanvas(onBoostComponents, 0, 0.25f).setEaseInOutSine();
            LeanTween.scale(onBoostComponents.gameObject, Vector3.one * 2, 0.5f).setEaseInOutSine();
            speedingLines.Play();
        }
        else
        {
            LeanTween.alphaCanvas(onBoostComponents, 1, 0.35f).setEaseInOutSine();
            LeanTween.scale(onBoostComponents.gameObject, Vector3.one, 0.25f).setEaseInOutSine();
            speedingLines.Stop();   
        }
    }

    private void OnCarCollision(Transform collisionTransform)
    {
        if (!isBoosting) return;
        
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(collisionTransform.position);
        CanvasGroup uiElement = plus500[0];
        uiElement.transform.position = screenPosition;

        uiElement.transform.localScale = Vector3.one * Random.Range (0.5f, 2.3f);
        LeanTween.moveY(uiElement.GetComponent<RectTransform>(), 300, 0.5f).setEaseInOutSine();
        LeanTween.alphaCanvas(uiElement, 1, 0.5f).setEaseInOutSine().setLoopPingPong(1);

        plus500.Remove(uiElement);
        plus500.Add(uiElement);
    }
}
