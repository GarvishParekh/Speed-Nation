using UnityEngine;
using UnityEngine.UI;

public class BoostController : MonoBehaviour
{
    [Header ("<size=15>SCRIPTABLE")]
    [SerializeField] private GameplayData gameplayData;

    [Space]
    [SerializeField] private Transform playerCarTransform;
    [SerializeField] private Transform laneCheckerTransform;
    [SerializeField] private Transform laneTriggerTransform;

    [Space]
    [SerializeField] private Transform[] lanesTransform;
    [SerializeField] private GameObject[] blinkingEffectObj;

    [Header ("<size=15>USER INTERFACE")]
    [SerializeField] private GameObject[] laneUiObject;
    [SerializeField] private GameObject[] boostUiObject;

    [Space]
    [SerializeField] private GameObject boostFillBarsUiObject;
    [SerializeField] private GameObject boostReleaseBarUiObject;
    [SerializeField] private Image boostReleaseBarImage;

    int laneIndex;
    int boostIndex;
    Vector3 NewPosition = Vector3.zero;


    private void OnEnable()
    {
        ActionManager.PlayerOnLane += IfPlayerOnLane;
        ActionManager.PlayCarSpawned += OnPlayerCarSpanned;
        ActionManager.TurtiolInitilize += OnTutriolInitiate;
    }

    private void OnDisable()
    {
        ActionManager.PlayerOnLane -= IfPlayerOnLane;
        ActionManager.PlayCarSpawned -= OnPlayerCarSpanned;
        ActionManager.TurtiolInitilize -= OnTutriolInitiate;
    }

    private void Start()
    {
        gameplayData.isBoosting = false;
        NewPosition.y = laneCheckerTransform.position.y;
        NewPosition.z = laneCheckerTransform.position.z;

        ChangeLane(true);
    }

    private void Update()
    {
        FollowPlayer();

        if (gameplayData.isBoosting)
        {
            gameplayData.boostTimer -= Time.deltaTime / 10f;
            boostReleaseBarImage.fillAmount = gameplayData.boostTimer;
            if (gameplayData.boostTimer < 0.0f)
            {
                gameplayData.isBoosting = false;

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
        AddBoost(gameplayData.addingBoostValue);
    }

    private void DisplayDesireUiObject(int selectedLaneIndex)
    {
        foreach (GameObject laneUi in laneUiObject)
        {
            if (laneUi.transform.GetSiblingIndex() == selectedLaneIndex)
            {
                laneUi.SetActive(true);
                blinkingEffectObj[laneUi.transform.GetSiblingIndex()].SetActive(true);
            }

            else 
            {
                laneUi.SetActive(false);
                blinkingEffectObj[laneUi.transform.GetSiblingIndex()].SetActive(false);
            }
        }
    }

    private void AddBoost(int boostScoreToAdd)
    {
        if (gameplayData.isBoosting) return;

        boostIndex += boostScoreToAdd;
        UpdateBoostUi();
        if (boostIndex >= 11)
        {
            boostIndex = 0;

            boostFillBarsUiObject.SetActive (false);
            boostReleaseBarUiObject.SetActive (true);

            boostReleaseBarImage.fillAmount = 1;

            gameplayData.boostTimer = 1;
            gameplayData.isBoosting = true;

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

    private void OnTutriolInitiate()
    {
        AddBoost(8);
    }
}
