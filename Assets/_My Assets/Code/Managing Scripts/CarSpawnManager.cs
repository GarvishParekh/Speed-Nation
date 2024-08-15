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

    [Header("<size=15>VALUES")]
    [SerializeField] Vector2 spawnMinMaxTime;
    [SerializeField] float timer;

    Vector3 spawnPoint;
    Vector2 edgeValue;

    private void Awake()
    {
        edgeValue.x = leftEdgeTransform.position.z;
        edgeValue.y = rightEdgeTransform.position.z;
    }

    private void Start()
    {
        ShuffleList(TrafficCars);
    }

    private void Update()
    {
        WaveTimer();
        DangerWave();
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        else
        {
            ResetTimer();
            spawnPoint.x = playerCar.position.x - 200;
            spawnPoint.y = 0.24f;
            spawnPoint.z = GetRandomNumber(edgeValue.x, edgeValue.y);

            Transform spawnedCar = TrafficCars[0];
            Rigidbody spawnedCarRb = spawnedCar.GetComponent<Rigidbody>();

            spawnedCar.gameObject.SetActive(true);
            spawnedCar.position = spawnPoint;
            spawnedCar.rotation = Quaternion.Euler(0, 90, 0);

            float randomSpeed = Random.Range(20, 25);
            spawnedCarRb.velocity = spawnedCarRb.transform.forward * randomSpeed;

            UpdateTrafficList(spawnedCar);
        }
    }

    private void ResetTimer ()
        => timer = Random.Range(spawnMinMaxTime.x, spawnMinMaxTime.y);

    private float GetRandomNumber (float min, float max)
    {
        float randomPosition = Random.Range(min, max);
        return randomPosition;
    }

    private void UpdateTrafficList(Transform spawnedCar)
    {
        TrafficCars.Remove(spawnedCar);
        TrafficCars.Add(spawnedCar);
    }
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
    {
        playerCar = _player;
    }

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
            spawnMinMaxTime = new Vector2(0.3f, 0.4f);
        }
        else
        {
            spawnMinMaxTime = new Vector2(0.8f, 1);
        }
    }

}
