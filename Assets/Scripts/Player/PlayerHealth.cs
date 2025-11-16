using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;
    public float damageCooldown;
    [SerializeField] bool canTakeDamage = true;

    void Start()
    {
        currentHealth = maxHealth;  // Inicia con todas las vidas
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        canTakeDamage = false;

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
        Debug.Log("Jugador ha muerto!");

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (canTakeDamage & collision.CompareTag("Laser"))
        {
            TakeDamage(1);
        }
    }
}

