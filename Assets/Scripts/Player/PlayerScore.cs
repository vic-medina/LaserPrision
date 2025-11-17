using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI finalScoreTxt;
    [SerializeField] private TextMeshProUGUI multTxt;
    private float timeAlive = 0f;
    public int score = 0;
    public int pointPerSecond = 1;
    [SerializeField] private int multiplier = 1;

    private float timer = 0f;
    private void Start()
    {

        playerHealth.OnPlayerDeath += StopScore;
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.isDead) return;

        // acumula tiempo vivo
        timeAlive += Time.deltaTime;

        // actualiza multiplicador según tiempoAlive
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

    void UpdateUI(int score, int multiplier)
    {
        scoreTxt.text = ""+score;
        multTxt.text = "x"+multiplier;
    }

    void StopScore()
    {
        timer = 0f;
        finalScoreTxt.text = "Score: "+score;
    }
}

