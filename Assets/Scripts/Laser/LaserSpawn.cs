using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class LaserSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LaserPool laserPool;

    [Header("SpawnSettings")]
    public float spawnInterval;   // Tiempo entre rayos
    public Vector2 spawnArea = new Vector2(20f, 20f); // Área en X y Z donde caerán
    public float increaseRate; // Tasa de aumento del spawnIntervalo
    public float reduceTimer; // Tasa de reducción del spawnIntervalo


    [Header("Angle Settings")]
    [SerializeField] private float maxAngleX; // máximo desvío en X
    [SerializeField] private float maxAngleZ; // máximo desvío en Z


    [Header("Height Settings")]
    public float spawnHeight;    // Altura desde la que aparece el rayo

    private float timer;

    private void Start()
    {
        StartCoroutine(IncreaseSpawnRate());
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnLaser();
            timer = 0f;
        }
    }

    void SpawnLaser()
    {
        //Posicion aleatoria
        float randomX = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
        float randomZ = Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f);
        Vector3 spawnPos = new Vector3(randomX, spawnHeight, randomZ);

        // Rotacion aleatoria
        float angleX = Random.Range(-maxAngleX, maxAngleX);
        float angleZ = Random.Range(-maxAngleZ, maxAngleZ);
        Quaternion rotation = Quaternion.Euler(angleX, 0f, angleZ);

        // Instancia el láser
        GameObject laser = laserPool.GetFromPool(spawnPos,rotation);
    }

    private IEnumerator IncreaseSpawnRate()
    {
        while (true)
        {
            yield return new WaitForSeconds(reduceTimer); // Cada 10 segundos
            spawnInterval = Mathf.Max(0.5f, spawnInterval - increaseRate); // Reduce el intervalo, pero no menos de 0.5 segundos
        }
    }
}
