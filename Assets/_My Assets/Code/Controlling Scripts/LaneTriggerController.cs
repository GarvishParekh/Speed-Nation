using UnityEngine;
using UnityEngine.UI;

public class LaneTriggerController : MonoBehaviour
{
    bool isInPosition = false;
    bool isPlayerBoosting = false;

    [SerializeField] private Image laneFillImage;
    [SerializeField] private Material laneMaterial;

    [SerializeField] private float timer = 0;
    [SerializeField] private float timeToChange = 0;

    private void OnEnable()
    {
        ActionManager.PlayerBoosting += OnPlayerBoosting;
    }

    private void OnDisable()
    {
        ActionManager.PlayerBoosting -= OnPlayerBoosting;
    }

    private void OnPlayerBoosting(bool check)
    {
        isPlayerBoosting = check;
    }

    private void Update()
    {
        if (isPlayerBoosting)
        {
            laneMaterial.SetInt("_canBlink", 0) ;
            return;
        }
        else
        {
            laneMaterial.SetInt("_canBlink", 1) ;
        }

        if (!isInPosition) return;

        if (timer < timeToChange) timer += Time.deltaTime;
        else
        {
            timer = 0;
            ActionManager.PlayerOnLane?.Invoke();
        }
        laneFillImage.fillAmount = timer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isInPosition = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag ("Player")) isInPosition = false;
    }
}
