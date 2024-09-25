using UnityEngine;

public class TutriolManager : MonoBehaviour
{
    [SerializeField] private TrafficSpawnManager trafficSpawner;

    [SerializeField] private GameObject laneInformationObj;
    [SerializeField] private GameObject boostInformationObj;
    [SerializeField] private GameObject moveToNextLane;
    [SerializeField] private GameObject healthInformationObj;
    [SerializeField] private GameObject tutriolCompletedObj;

    [Space]
    [SerializeField] private GameObject uiDimmer;

    [Space]
    [SerializeField] private Transform highlitedHolder;
    [SerializeField] private Transform afterUseHolder;

    [Space]
    [SerializeField] private Transform laneUiComponent;
    [SerializeField] private Transform boostUiComponent;
    [SerializeField] private Transform healthUiComponent;

    public int tutriolIndex = 0;
    Transform[] tipComponents = new Transform[3];

    private void Start()
    {
        int completedIndex = PlayerPrefs.GetInt(ConstantKeys.TUTORIAL_COMPLETED, 0);
        tutriolIndex = completedIndex;
        if (completedIndex == 1)
        {
            gameObject.SetActive(false);    
            return;
        }
        
        tipComponents[0] = laneUiComponent;
        tipComponents[1] = boostUiComponent;
        tipComponents[2] = healthUiComponent;
        
        trafficSpawner.gameObject.SetActive(false);
        ActionManager.TurtiolInitilize?.Invoke();
    }

    private void OnEnable()
    {
        ActionManager.countDownCompleted += ShowLaneInformation;
        ActionManager.PlayerOnLane += ShowBoostInformation;
        ActionManager.PlayerBoosting += ShowHealthInformation;
    }

    private void OnDisable()
    {
        ActionManager.countDownCompleted -= ShowLaneInformation;
        ActionManager.PlayerOnLane -= ShowBoostInformation;
        ActionManager.PlayerBoosting -= ShowHealthInformation;
    }

    private void ShowLaneInformation()
    {
        ShowDesireTip(laneInformationObj, laneUiComponent);
    }

    bool boostPointAdded = false;
    private void ShowBoostInformation()
    {
        if (boostPointAdded) return;
        ShowDesireTip(boostInformationObj, boostUiComponent);

        boostPointAdded = true;
    }

    public void _ShowMoveToNexLaneInformation()
    {
        ShowDesireTip(moveToNextLane, laneUiComponent);
    }

    private void ShowHealthInformation(bool check)
    {
        if (check)
        {
            ShowDesireTip(healthInformationObj, healthUiComponent);
        }
        else
        {
            ShowDesireTip(tutriolCompletedObj, null);
        }
    }

    public void _TutriolCompleted()
    {
        PlayerPrefs.SetInt(ConstantKeys.TUTORIAL_COMPLETED, 1);
    }


    private void ShowDesireTip(GameObject desireTip, Transform uiComponent)
    {
        HideAllTips();
        uiDimmer.SetActive(true);

        desireTip.SetActive(true);
        if (uiComponent != null)
        {
            uiComponent.transform.SetParent(highlitedHolder);
        }

        Time.timeScale = 0;
    }

    public void HideAllTips()
    {
        uiDimmer.SetActive(false);

        laneInformationObj.SetActive(false);
        boostInformationObj.SetActive(false);
        moveToNextLane.SetActive(false);
        healthInformationObj.SetActive(false);
        
        foreach (Transform tipComponent in tipComponents)
        {
            tipComponent.transform.SetParent(afterUseHolder);
        }

        Time.timeScale = 1;
    }
}
