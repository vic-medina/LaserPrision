using System.Collections;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LaserPool laserPool;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Rigidbody playerRb;

    [Header("SpawnSettings")]
    public float spawnInterval = 2f;
    public Vector2 spawnArea = new Vector2(20f, 20f);
    public float increaseRate = 0.1f;
    public float reduceTimer = 5f;

    [Header("Angle Settings")]
    [SerializeField] private float maxAngleX;
    [SerializeField] private float maxAngleZ;
    [SerializeField] private float playerMaxAngleX;
    [SerializeField] private float playerMaxAngleZ;

    [Header("Height Settings")]
    public float spawnHeight = 10f;

    [Header("Idle Settings")]
    public float idleThreshold = 3f;
    [SerializeField] private float idleTimer = 0f;

    private float timer;

    private void Start()
    {
        StartCoroutine(IncreaseSpawnRate());
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

        CheckIdleAndSpawn();
    }

    void SpawnLaser()
    {
        //Posicion aleatoria dentro del area
        float randomX = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
        float randomZ = Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f);
        Vector3 spawnPos = new Vector3(randomX, spawnHeight, randomZ);

        //Angulo aleatorio
        float angleX = Random.Range(-maxAngleX, maxAngleX);
        float angleZ = Random.Range(-maxAngleZ, maxAngleZ);
        Quaternion rotation = Quaternion.Euler(angleX, 0f, angleZ);

        //Se obtiene del pool un laser
        laserPool.GetFromPool(spawnPos, rotation);
    }

    void SpawnLaserOnPlayer(Vector3 playerPos)
    {
        //Spawn de laser encima del player
        Vector3 spawnPos = new Vector3(playerPos.x, spawnHeight, playerPos.z);
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);

        laserPool.GetFromPool(spawnPos, rotation);
    }

    private void CheckIdleAndSpawn()
    {
        //Verificar si el player esta en movimiento
        if (playerRb == null) return;

        // velocidad horizontal del player
        Vector3 horizontalVel = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);

        if (horizontalVel.magnitude < 0.1f) // Si el player no tiene movimiento, contador de inactividad aumenta    
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleThreshold) // Si el contador supera el umbral, spawn de laser encima del player
            {
                SpawnLaserOnPlayer(playerRb.transform.position);
                idleTimer = 0f;
            }
        }
        else
        {
            idleTimer = 0f;
        }
    }

    private IEnumerator IncreaseSpawnRate() // Disminuye el intervalo de spawn con el tiempo
    {
        while (true)
        {
            yield return new WaitForSeconds(reduceTimer);
            spawnInterval = Mathf.Max(0.5f, spawnInterval - increaseRate);
        }
    }
}
