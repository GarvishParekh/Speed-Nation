using UnityEngine;
using UnityEngine.UI;

public class BoostController : MonoBehaviour
{
    [SerializeField] private Transform playerCarTransform;
    [SerializeField] private Transform laneCheckerTransform;
    [SerializeField] private Transform laneTriggerTransform;

    [Space]
    [SerializeField] private Transform[] lanesTransform;

    [Header ("<size=15>USER INTERFACE")]
    [SerializeField] private GameObject[] laneUiObject;
    [SerializeField] private GameObject[] boostUiObject;

    [Space]
    [SerializeField] private GameObject boostFillBarsUiObject;
    [SerializeField] private GameObject boostReleaseBarUiObject;
    [SerializeField] private Image boostReleaseBarImage;

    [Header("<size=15>BOOST VALUE")]
    [SerializeField] private float boostTimer = 1;
    [SerializeField] private int addingBoostValue = 1;

    int laneIndex;
    int boostIndex;
    bool activateBoost = false;
    Vector3 NewPosition = Vector3.zero;


    private void OnEnable()
    {
        ActionManager.PlayerOnLane += IfPlayerOnLane;
        ActionManager.PlayCarSpawned += OnPlayerCarSpanned;
    }

    private void OnDisable()
    {
        ActionManager.PlayerOnLane -= IfPlayerOnLane;
        ActionManager.PlayCarSpawned -= OnPlayerCarSpanned;
    }

    private void Start()
    {
        NewPosition.y = laneCheckerTransform.position.y;
        NewPosition.z = laneCheckerTransform.position.z;

        ChangeLane(true);
    }

    private void Update()
    {
        FollowPlayer();

        if (activateBoost)
        {
            boostTimer -= Time.deltaTime / 10f;
            boostReleaseBarImage.fillAmount = boostTimer;
            if (boostTimer < 0.0f)
            {
                activateBoost = false;
                boostIndex = 0;
                boostFillBarsUiObject.SetActive(true);
                boostReleaseBarUiObject.SetActive(false);

                ActionManager.PlayerBoosting?.Invoke(false);
                UpdateBoostUi();
            }
        }
    }

    private void OnPlayerCarSpanned (Transform playerCar)
    {
        playerCarTransform = playerCar;
    }

    private void FollowPlayer()
    {
        NewPosition.x = playerCarTransform.position.x;  
        laneCheckerTransform.position = NewPosition;
    }

    private void ChangeLane(bool initial)
    {
        if (initial) laneIndex = 0;
        else laneIndex = Random.Range (0, lanesTransform.Length);

        laneTriggerTransform.position = lanesTransform[laneIndex].position;

        // update ui
        DisplayDesireUiObject(laneIndex);
    }

    private void IfPlayerOnLane()
    {
        ChangeLane(false);
        AddBoost();
    }

    private void DisplayDesireUiObject(int selectedLaneIndex)
    {
        foreach (GameObject laneUi in laneUiObject)
        {
            if (laneUi.transform.GetSiblingIndex() == selectedLaneIndex) 
                laneUi.SetActive (true);

            else laneUi.SetActive (false);
        }
    }

    private void AddBoost()
    {
        if (activateBoost) return;

        boostIndex += addingBoostValue;
        UpdateBoostUi();
        if (boostIndex >= 11)
        {
            boostIndex = 0;

            boostFillBarsUiObject.SetActive (false);
            boostReleaseBarUiObject.SetActive (true);

            boostReleaseBarImage.fillAmount = 1;

            boostTimer = 1;
            activateBoost = true;

            ActionManager.PlayerBoosting?.Invoke(true);
        }
    }

    private void UpdateBoostUi()
    {
        foreach (GameObject boostUi in boostUiObject)
        {
            if (boostUi.transform.GetSiblingIndex() < boostIndex)
            {
                boostUi.SetActive(true);
            }
            else boostUi.SetActive(false);
        }
    }
}
