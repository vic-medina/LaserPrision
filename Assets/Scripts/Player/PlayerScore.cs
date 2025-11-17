using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI finalScoreTxt;
    [SerializeField] private TextMeshProUGUI multTxt;
    public PlayerHealth playerHealth;

    [Header("Score settings")]
    [SerializeField] private int score = 0;
    [SerializeField] private int pointPerSecond = 1;
    [SerializeField] private int multiplier = 1;
    [SerializeField] private float timeAlive = 0f;

    private float timer = 0f;

    private void Start()
    {
        playerHealth.OnPlayerDeath += StopScore;
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.isDead) return;

        timeAlive += Time.deltaTime;

        if (timeAlive < 15f)
            multiplier = 1;
        else if (timeAlive < 40f)
            multiplier = 2;
        else if (timeAlive < 90f)
            multiplier = 3;
        else
            multiplier = 5;

        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            score += pointPerSecond * multiplier;
            timer = 0f;
            UpdateUI(score, multiplier);
        }
    }

    void UpdateUI(int score, int multiplier) // Actualiza la UI del puntaje
    {
        scoreTxt.text = ""+score;
        multTxt.text = "x"+multiplier;
    }

    void StopScore() // Detiene el puntaje al morir
    {
        timer = 0f;
        finalScoreTxt.text = "Score: "+score;
    }
}

