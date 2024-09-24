using UnityEngine;

public class LaneTriggerController : MonoBehaviour
{
    bool isInPosition = false;

    [SerializeField] private float timer = 0;
    [SerializeField] private float timeToChange = 0;

    private void Update()
    {
        if (!isInPosition) return;

        if (timer < timeToChange) timer += Time.deltaTime;
        else
        {
            timer = 0;
            ActionManager.PlayerOnLane?.Invoke();
        }
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
