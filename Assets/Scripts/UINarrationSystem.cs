using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINarrationSystem : MonoBehaviour, IObserver
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        
        UpdateScore(0);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }


    public void OnNotify(string eventName, object data)
    {
        switch (eventName)
        {
            case "ScoreChanged":
                UpdateScore((int)data);
                break;
            case "GameOver":
                ShowGameOver();
                break;
            case "GamePaused":
                if (GameManager.Instance.IsGameOver) break;
                PauseGame();
                break;
            case "GameResumed":
                if (GameManager.Instance.IsGameOver) break;
                ResumeGame();
                break;
        }
    }

    private void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    private void ShowGameOver()
    {
        Debug.Log("Game OVERRRR");
        Time.timeScale = 0;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (gameOverScoreText != null)
            {
                gameOverScoreText.text = $"Final Score: {gameManager.GetGameState<int>("Score")}";
            }
        }
    }

    private void PauseGame()
    {
        gameManager.SetGameState("IsPaused", true);
        Time.timeScale = 0;
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }


    public void OnRestartButtonClicked()
    {
        if (gameManager != null)
        {
            gameManager.RestartGame();
        }
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }


    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        gameManager.SetGameState("IsPaused", false);
    }

    public void OnHomeButtonClicked()
    {
        if (gameManager != null)
        {
            gameManager.GoToMainMenu();
        }
    }
    private void OnEnable(){
        GameManager.Instance.AddObserver(this);
    }
    private void OnDisable()
    {
        GameManager.Instance.RemoveObserver(this);
    }
}
