using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;   // Referencia al script de salud
    public Image[] hearts;              // Array de imágenes de corazones

    void Update()
    {
        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth.currentHealth)
                hearts[i].enabled = true;   // Corazón encendido
            else
                hearts[i].enabled = false;  // Corazón apagado
        }
    }
}
