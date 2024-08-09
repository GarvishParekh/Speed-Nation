using UnityEngine;

public class CarSpawnController : MonoBehaviour
{
    int selectedCar = 0;
    [SerializeField] private GameObject[] allCars;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private CarSpawnManager carSpawnManager;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform carRotationTransform;
    [SerializeField] private Transform carHolder;
    [SerializeField] private Transform rotationDisk;

    private void Start()
    {
        SpawnCar();
    }

    private void SpawnCar()
    {
        selectedCar = PlayerPrefs.GetInt(ConstantKeys.SELECTED_CAR, 0);
        carHolder = Instantiate(allCars[selectedCar], spawnPoint.position, Quaternion.Euler(0, -90, 0)).transform;

        VehicleCollisionController cv = carHolder.GetComponent<VehicleCollisionController>();
        CarController cc = carHolder.GetComponent<CarController>();

        cv.SetCameraController(cameraController);
        cc.SetRotationalDisk(rotationDisk);

        cameraController.SetPlayer(cc.GetRotationTransform());
        carSpawnManager.SetPlayer(carHolder);   
    }
}
