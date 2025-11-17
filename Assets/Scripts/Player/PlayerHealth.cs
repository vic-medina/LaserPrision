using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;
    public float damageCooldown;
    [SerializeField] bool canTakeDamage = true;
    public delegate void PlayerDeathEvent();
    public event PlayerDeathEvent OnPlayerDeath;
    public bool isDead = false;

    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration;
    [SerializeField] private int flashCount;

    [SerializeField] private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        flashCount = Mathf.FloorToInt(damageCooldown / (flashDuration * 2));
    }

    public void TakeDamage(int amount)
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

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private void Die()
    {
        if (isDead) return; 
        isDead = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        OnPlayerDeath?.Invoke();
    }

    private IEnumerator FlashDamage()
    {
        for (int i = 0; i < flashCount; i++)
        {
            playerRenderer.material.color = damageColor;
            yield return new WaitForSeconds(flashDuration);
            playerRenderer.material.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (canTakeDamage & collision.CompareTag("Laser"))
        {
            TakeDamage(1);
        }
    }
}

