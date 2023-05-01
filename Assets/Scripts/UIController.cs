using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject gamePausedUI;
    public GameObject healthUI;
    public GameObject timerUI;
    public GameObject scoreUI;
    public ScoreManager scoreManager;

    public Text scoreText;

    public RectTransform healthBar;

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        FindObjectOfType<Player>().OnDeath += OnGameOver;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (player != null)
        {
            scoreText.text = ScoreManager.score.ToString("000000");
            float healthPercent = player.m_health / player.startingHealth;
            healthBar.localScale = new Vector3(healthPercent, 1, 1);
        }

    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        if (!gameOverUI.activeInHierarchy)
        {
            gamePausedUI.SetActive(true);
            timerUI.SetActive(false);
            scoreUI.SetActive(false);
            healthUI.SetActive(false);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        gamePausedUI.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnGameOver()
    {
        Time.timeScale = 0.0f;
        gameOverUI.SetActive(true);
        timerUI.SetActive(false);
        scoreUI.SetActive(false);
        healthUI.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        Timer timerUITimer = timerUI.GetComponent<Timer>();
        timerUITimer.ResetTimer();
        scoreManager.ResetScore();
        SceneManager.LoadScene("Game");
    }
}
