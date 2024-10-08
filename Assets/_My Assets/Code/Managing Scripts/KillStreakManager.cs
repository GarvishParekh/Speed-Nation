using UnityEngine;

public class KillStreakManager : MonoBehaviour
{
    [SerializeField] private GameplayUiController gameplayUiController;
    [SerializeField] private int killStreakCount = 0;
    [SerializeField] private float timer = 2;
    [SerializeField] private float resetTime = 5;

    bool startCounting = false;

    private void OnEnable()
    {
        ActionManager.TrafficKilled += OnTrafficKill;
    }

    private void OnDisable()
    {
        ActionManager.TrafficKilled -= OnTrafficKill;
    }

    private void Update()
    {
        if (!startCounting) return;

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            killStreakCount = 0;
            startCounting = false;

            ActionManager.KillStreakCouterReset?.Invoke();    
        }
        gameplayUiController.UpdateKillStreakTimerText(timer);
    }

    private void OnTrafficKill()
    {
        killStreakCount++;
        timer = resetTime;

        startCounting = true;
        ActionManager.UpdateKillStreak(killStreakCount);
    }
}
