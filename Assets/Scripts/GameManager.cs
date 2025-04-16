using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : Subject
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private bool isGameOver
    {
        get { return GetGameState<bool>("IsGameOver"); }
        set { SetGameState("IsGameOver", value); }
    }

    private bool isGamePaused
    {
        get { return GetGameState<bool>("IsPaused"); }
        set { SetGameState("IsPaused", value); }
    }   
    public bool IsGameOver
    {
        get { return isGameOver; }
    }
    public bool IsGamePaused
    {
        get { return isGamePaused; }
    }

    private Dictionary<string, object> gameState = new Dictionary<string, object>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeGameState();
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState.ContainsKey("IsPaused") && (bool)gameState["IsPaused"])
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    private void InitializeGameState()
    {
        SetGameState("Score", 0);
    }

    public void SetGameState(string key, object value)
    {
        gameState[key] = value;
        NotifyObservers(key, value);
    }

    public T GetGameState<T>(string key)
    {
        if (gameState.ContainsKey(key))
        {
            return (T)gameState[key];
        }
        return default(T);
    }

    public void StartGame()
    {
        InitializeGameState();
        NotifyObservers("GameStarted", null);
    }

    public void GameOver()
    {
        SetGameState("IsGameOver", true);
        NotifyObservers("GameOver", null);
    }

    public void PauseGame()
    {
        NotifyObservers("GamePaused", null);
    }
    public void ResumeGame()
    {
        NotifyObservers("GameResumed", null);
    }
    public void AddScore(int points)
    {
        int currentScore = GetGameState<int>("Score");
        SetGameState("Score", currentScore + points);
        NotifyObservers("ScoreChanged", currentScore + points);
    }

    public void EnemySpawned(GameObject enemy)
    {
        NotifyObservers("EnemySpawned", enemy);
    }

    public void EnemyDestroyed(GameObject enemy)
    {
        NotifyObservers("EnemyDestroyed", enemy);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("HomeScence");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

} 