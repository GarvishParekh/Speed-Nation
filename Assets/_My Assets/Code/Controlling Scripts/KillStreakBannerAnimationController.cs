using UnityEngine;
using UnityEngine.UI;

public class KillStreakBannerAnimationController : MonoBehaviour
{
    private Image bannerImage;
    [SerializeField] private float animationSpeed;

    private void Awake() => bannerImage = GetComponent<Image>();

    private void OnEnable() => Animate();

    private void Animate()
    {
        bannerImage.fillAmount = 0;
        LeanTween.value(gameObject, (float newFillAmount) => bannerImage.fillAmount = newFillAmount,
            bannerImage.fillAmount, // Current fill amount
            1, // Target fill amount
            animationSpeed // Duration of the animation
        );
    }
}
