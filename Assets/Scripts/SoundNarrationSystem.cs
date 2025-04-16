using UnityEngine;
using System.Collections;

public class SoundNarrationSystem : MonoBehaviour, IObserver
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource narrationSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip gameStartMusic;
    [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip enemySpawnSound;
    [SerializeField] private AudioClip scoreSound;

    [Header("Narration Clips")]
    [SerializeField] private AudioClip gameStartNarration;
    [SerializeField] private AudioClip gameOverNarration;
    [SerializeField] private AudioClip playerDeathNarration;
    [SerializeField] private AudioClip[] scoreNarrations;

    private void Start()
    {
        // Đăng ký làm observer
        GameManager.Instance.AddObserver(this);
    }

    private void OnDestroy()
    {
        // Hủy đăng ký observer khi object bị destroy
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RemoveObserver(this);
        }
    }

    public void OnNotify(string eventName, object data)
    {
        switch (eventName)
        {
            case "GameStarted":
                HandleGameStart();
                break;
            case "GameOver":
                HandleGameOver();
                break;
            case "PlayerDied":
                HandlePlayerDeath();
                break;
            case "ScoreChanged":
                HandleScoreChange((int)data);
                break;
            case "EnemySpawned":
                PlaySound(enemySpawnSound);
                break;
            case "GamePaused":
                HandleGamePause();
                break;
            case "GameResumed":
                HandleGameResume();
                break;
        }
    }

    private void HandleGameStart()
    {
        // Phát nhạc nền
        if (musicSource != null && gameStartMusic != null)
        {
            musicSource.clip = gameStartMusic;
            musicSource.Play();
        }

        // Phát narration
        if (narrationSource != null && gameStartNarration != null)
        {
            narrationSource.clip = gameStartNarration;
            narrationSource.Play();
        }
    }

    private void HandleGameOver()
    {
        // Phát nhạc game over
        if (musicSource != null && gameOverMusic != null)
        {
            musicSource.clip = gameOverMusic;
            musicSource.Play();
        }

        // Phát narration game over
        if (narrationSource != null && gameOverNarration != null)
        {
            narrationSource.clip = gameOverNarration;
            narrationSource.Play();
        }
    }

    private void HandlePlayerDeath()
    {
        // Phát âm thanh chết
        PlaySound(playerDeathSound);

        // Phát narration
        if (narrationSource != null && playerDeathNarration != null)
        {
            narrationSource.clip = playerDeathNarration;
            narrationSource.Play();
        }
    }

    private void HandleScoreChange(int score)
    {
        // Phát âm thanh ghi điểm
        PlaySound(scoreSound);

        // Phát narration tương ứng với điểm số
        if (narrationSource != null && scoreNarrations != null && scoreNarrations.Length > 0)
        {
            int narrationIndex = Mathf.Min(score / 1000, scoreNarrations.Length - 1);
            narrationSource.clip = scoreNarrations[narrationIndex];
            narrationSource.Play();
        }
    }

    private void HandleGamePause()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
        if (narrationSource != null)
        {
            narrationSource.Pause();
        }
    }

    private void HandleGameResume()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
        if (narrationSource != null)
        {
            narrationSource.UnPause();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
} 