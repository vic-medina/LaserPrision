using System.Collections;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LaserPool laserPool;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Rigidbody playerRb; // referencia al rigidbody del player

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
    public float idleThreshold = 3f; // segundos quieto antes de que caiga un láser
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

        // Spawn aleatorio normal
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnLaser();
            timer = 0f;
        }

        // Lógica de quieto
        CheckIdleAndSpawn();
    }

    void SpawnLaser()
    {
        float randomX = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
        float randomZ = Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f);
        Vector3 spawnPos = new Vector3(randomX, spawnHeight, randomZ);

        float angleX = Random.Range(-maxAngleX, maxAngleX);
        float angleZ = Random.Range(-maxAngleZ, maxAngleZ);
        Quaternion rotation = Quaternion.Euler(angleX, 0f, angleZ);

        laserPool.GetFromPool(spawnPos, rotation);
    }

    void SpawnLaserOnPlayer(Vector3 playerPos)
    {
        Vector3 spawnPos = new Vector3(playerPos.x, spawnHeight, playerPos.z);
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);

        laserPool.GetFromPool(spawnPos, rotation);
    }

    private void CheckIdleAndSpawn()
    {
        if (playerRb == null) return;

        // velocidad horizontal del player
        Vector3 horizontalVel = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);

        if (horizontalVel.magnitude < 0.1f) // casi quieto
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleThreshold)
            {
                SpawnLaserOnPlayer(playerRb.transform.position);
                idleTimer = 0f; // reinicia para que no caiga cada frame
            }
        }
        else
        {
            idleTimer = 0f; // si se mueve, reinicia el contador
        }
    }

    private IEnumerator IncreaseSpawnRate()
    {
        while (true)
        {
            yield return new WaitForSeconds(reduceTimer);
            spawnInterval = Mathf.Max(0.5f, spawnInterval - increaseRate);
        }
    }
}
