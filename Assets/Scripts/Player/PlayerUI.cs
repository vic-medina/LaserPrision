using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;   // Referencia al script de salud
    public Image[] hearts;              // Array de imágenes de corazones
    public GameObject scorePanel;
    public GameObject gameOverPanel;    // Panel de Game Over

    private void Start()
    {
        gameOverPanel.SetActive(false);
        playerHealth.OnPlayerDeath += ShowGameOver;
    }

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

    private void ShowGameOver()
    {
        scorePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
