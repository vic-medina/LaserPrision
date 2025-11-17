using System.Collections;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LaserPool laserPool;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("SpawnSettings")]
    public float spawnInterval;   // Tiempo entre rayos
    public float followPlayerSpawnInterval; // Tiempo entre rayos cuando sigue al jugador
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
        StartCoroutine(LaserFollowPlayer());
    }
    private void Update()
    {
        if (playerHealth != null && playerHealth.isDead)
        {
            StopAllCoroutines();
            return;
        }
            

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

    void SpawnLasertoPlayer(Vector3 playerPos)
    {
        // Posición de spawn centrada en el jugador
        float randomX = Random.Range(-playerPos.x / 2f, playerPos.x / 2f);
        float randomZ = Random.Range(-playerPos.z / 2f, playerPos.z / 2f);
        Vector3 spawnPos = new Vector3(playerPos.x + randomX, spawnHeight, playerPos.z + randomZ);

        /*Vector3 spawnPos = new Vector3(playerPos.x, spawnHeight, playerPos.z);*/

        float angleX = Random.Range(-maxAngleX, maxAngleX);
        float angleZ = Random.Range(-maxAngleZ, maxAngleZ);
        Quaternion rotation = Quaternion.Euler(angleX, 0f, angleZ);
        // Instancia el láser
        GameObject laser = laserPool.GetFromPool(spawnPos, rotation);
    }

    private IEnumerator IncreaseSpawnRate()
    {
        while (true)
        {
            yield return new WaitForSeconds(reduceTimer); 
            spawnInterval = Mathf.Max(0.5f, spawnInterval - increaseRate);
        }
    }

    private IEnumerator LaserFollowPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        while (true)
        {
            yield return new WaitForSeconds(followPlayerSpawnInterval);
            if (player != null)
            {
                Vector3 playerPos = player.transform.position;
                SpawnLasertoPlayer(playerPos);
            }
        }
    }
}
