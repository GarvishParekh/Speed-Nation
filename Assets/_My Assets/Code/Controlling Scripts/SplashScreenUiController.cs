using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenUiController : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainCanvasGroup;

    [SerializeField] private CanvasGroup nineIntCg;
    [SerializeField] private CanvasGroup nineCg;
    [SerializeField] private CanvasGroup squareCg;
    [SerializeField] private CanvasGroup gamesCg;

    private void Start()
    {
        Application.targetFrameRate = 60;
        nineIntCg.alpha = 0;
        nineCg.alpha = 0;
        squareCg.alpha = 0;
        gamesCg.alpha = 0;
        RevealTittle();
    }

    private void RevealTittle()
    {
        LeanTween.alphaCanvas(nineIntCg, 1, 0.25f).setDelay(1f).setEaseInOutSine().setOnComplete(() =>
        {
            LeanTween.alphaCanvas(nineCg, 1, 0.25f).setEaseInOutSine().setOnComplete(() =>
            {
                LeanTween.alphaCanvas(squareCg, 1, 0.25f).setEaseInOutSine().setOnComplete(() =>
                {
                    LeanTween.alphaCanvas(gamesCg, 1, 0.25f).setEaseInOutSine().setOnComplete(() =>
                    {
                        LeanTween.alphaCanvas(mainCanvasGroup, 0, 1f).setDelay(3f).setEaseInOutSine().setOnComplete(() =>
                        {
                            SceneManager.LoadScene(ConstantKeys.SCENE_MAIN_MENU);
                        });
                    });
                });
            });
        });
    }
}
