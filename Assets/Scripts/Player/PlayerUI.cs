using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Image[] hearts;
    public GameObject scorePanel;
    public GameObject gameOverPanel;
    public GameObject tutorialPanel;

    private void Start()
    {
        tutorialPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        playerHealth.OnPlayerDeath += ShowGameOver;
    }

    void Update()
    {
        CheckInput();
        UpdateHearts();
    }

    private void CheckInput() // Cierra el panel de tutorial al presionar cualquier tecla
    {
        if (tutorialPanel.activeSelf && Input.anyKeyDown)
        {
            tutorialPanel.SetActive(false);
        }
    }

    void UpdateHearts() // Actualiza la vida del jugador en la UI
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth.currentHealth)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }

    private void ShowGameOver() // Muestra el panel de Game Over
    {
        scorePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void Restart() // Reinicia la escena actual
    {
        SceneManager.LoadScene(0);
    }
}
