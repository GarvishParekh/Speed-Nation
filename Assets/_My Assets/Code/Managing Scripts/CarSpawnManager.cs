using UnityEngine;
using System.Collections.Generic;

public class CarSpawnManager : MonoBehaviour
{
    [Header ("<size=15>COMPONENTS")]
    [SerializeField] private Transform playerCar;
    [SerializeField] private Transform leftEdgeTransform;
    [SerializeField] private Transform rightEdgeTransform;
    [SerializeField] private List<Transform> TrafficCars = new List<Transform>();
    [SerializeField] private List<DestroyedVehicleController> afterCollision = new List<DestroyedVehicleController>();
    [SerializeField] private List<ParticleSystem> carExplosionParticles = new List<ParticleSystem>();
    [SerializeField] private List<Transform> trafficZSpawnPoints = new List<Transform>();
    [SerializeField] private Transform tollPrefab;

    [Header("<size=15>VALUES")]
    [SerializeField] Vector2 spawnMinMaxTime;
    [SerializeField] Vector2 normalSpawnRate;
    [SerializeField] Vector2 dangerSpawnRate;
    [SerializeField] float timer;

    Vector3 trafficSpawnPoint;
    bool canSpawn = true;

    private void OnEnable()
    {
        CarController.TollHit += OnTollHit;
    }

    private void OnDisable()
    {
        CarController.TollHit -= OnTollHit;
    }

    private void Start()
        => ShuffleList(TrafficCars);
    public void ShuffleList<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void Update()
    {
        if (!canSpawn) return;

        SpawnToll();

        WaveTimer();
        DangerWave();
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        else
        {
            ResetTimer();
            trafficSpawnPoint.x = playerCar.position.x - 200;
            trafficSpawnPoint.y = 0.24f;
            int randomIndex = Random.Range(0, trafficZSpawnPoints.Count);
            trafficSpawnPoint.z = trafficZSpawnPoints[randomIndex].position.z;

            Transform spawnedCar = TrafficCars[0];
            Rigidbody spawnedCarRb = spawnedCar.GetComponent<Rigidbody>();

            spawnedCar.gameObject.SetActive(true);
            spawnedCar.position = trafficSpawnPoint;
            spawnedCar.rotation = Quaternion.Euler(0, 90, 0);

            float randomSpeed = Random.Range(20, 25);
            spawnedCarRb.velocity = spawnedCarRb.transform.forward * randomSpeed;

            UpdateTrafficList(spawnedCar);
        }
    }

    private void ResetTimer ()
        => timer = Random.Range(spawnMinMaxTime.x, spawnMinMaxTime.y);

    private void UpdateTrafficList(Transform spawnedCar)
    {
        TrafficCars.Remove(spawnedCar);
        TrafficCars.Add(spawnedCar);
    }
    

    Vector3 particleOffset = new Vector3(0, 1, 0);
    public void SpawnDestroyedCar(Transform trafficCar)
    {
        DestroyedVehicleController dv = afterCollision[0].GetComponent<DestroyedVehicleController>();

        ParticleSystem explosionParticle = carExplosionParticles[0];
        carExplosionParticles.Remove(explosionParticle);
        carExplosionParticles.Add(explosionParticle);

        explosionParticle.transform.position = trafficCar.position + particleOffset;
        explosionParticle.Play(true);

        dv.gameObject.SetActive(true);
        dv.OnSpawn();

        dv.transform.SetPositionAndRotation
        (
            trafficCar.position, 
            trafficCar.rotation
        );

        afterCollision.Remove(dv); 
        afterCollision.Add(dv); 
    }

    public void SetPlayer(Transform _player)
        => playerCar = _player;

    [Header ("<size=15>WAVE COUNTER")]
    [SerializeField] private float waveIncomingTimer = 0;
    [SerializeField] private float waveTime = 50;
    [SerializeField] private float dangerWaveTime = 0;
    private void WaveTimer()
    {
        if (waveIncomingTimer < waveTime)
        {
            waveIncomingTimer += Time.deltaTime;
        }
        else
        {
            WarningAnimation();
            waveIncomingTimer = 0;
            dangerWaveTime = 20;
        }
    }

    [SerializeField] GameObject mainholder;
    [SerializeField] CanvasGroup information;

    [SerializeField] private float spawnTollTimer = 0;
    [SerializeField] private float spawnTollTimerThreshold = 60f;
    private void WarningAnimation()
    {
        mainholder.transform.localScale = new Vector3 (0, 1, 1);
        information.alpha = 0;
        LeanTween.scale(mainholder, Vector3.one, 0.8f).setEaseInOutElastic().setOnComplete(() =>
        {
            LeanTween.alphaCanvas(information, 1, 0.25f).setOnComplete(() =>
            {
                LeanTween.alphaCanvas(information, 0, 0.25f).setDelay(2).setOnComplete(() =>
                {
                    LeanTween.scale(mainholder, new Vector3(0, 1, 1), 0.8f).setEaseInOutElastic();
                });
            });
        });
    }

    private void DangerWave()
    {
        if (dangerWaveTime > 0)
        {
            dangerWaveTime -= Time.deltaTime;
            spawnMinMaxTime = dangerSpawnRate;
        }
        else
        {
            spawnMinMaxTime = normalSpawnRate;
        }
    }

    private void SpawnToll()
    {
        if (spawnTollTimer > spawnTollTimerThreshold)
        {
            if (canSpawn)
            {
                trafficSpawnPoint.x = playerCar.position.x - 200;
                trafficSpawnPoint.y = 0;
                trafficSpawnPoint.z = -115f;
                tollPrefab.position = trafficSpawnPoint;
            }
            canSpawn = false;
        }
        else
        {
            spawnTollTimer += Time.deltaTime;
        }
    }

    private void OnTollHit()
    {
        canSpawn = true;
        spawnTollTimer = 0;
    }

}
