using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Renderer playerRenderer;
    
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    public int currentHealth;
    [SerializeField] private float damageCooldown;
    [SerializeField] private bool canTakeDamage = true;
    public bool isDead = false;

    [Header("Damage Feedback")]
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration;
    [SerializeField] private int flashCount;
    [SerializeField] private Color originalColor;

    public delegate void PlayerDeathEvent(); // Define el evento de muerte del jugador
    public event PlayerDeathEvent OnPlayerDeath;

    void Start()
    {
        currentHealth = maxHealth;
        flashCount = Mathf.FloorToInt(damageCooldown / (flashDuration * 2));
    }

    public void TakeDamage(int amount) // Maneja el daño recibido por el jugador
    {
        currentHealth -= amount;
        canTakeDamage = false;

        StartCoroutine(FlashDamage());

        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(DamageCooldown());
    }

    private IEnumerator DamageCooldown() // Controla el tiempo de invulnerabilidad después de recibir daño
    {
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private void Die() // Maneja la muerte del jugador
    {
        if (isDead) return; 
        isDead = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        OnPlayerDeath?.Invoke();
    }

    private IEnumerator FlashDamage() // Efecto visual de parpadeo al recibir daño
    {
        for (int i = 0; i < flashCount; i++)
        {
            playerRenderer.material.color = damageColor;
            yield return new WaitForSeconds(flashDuration);
            playerRenderer.material.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void OnTriggerEnter(Collider collision) // Detecta colisiones con láseres enemigos
    {
        if (canTakeDamage & collision.CompareTag("Laser"))
        {
            TakeDamage(1);
        }
    }
}

