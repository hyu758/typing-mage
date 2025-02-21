using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private int currentScore;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    void Start()
    {
        Instance = this;
        currentScore = 0;
        scoreText.text = currentScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateScore(int score)
    {
        currentScore += score;
        scoreText.text = currentScore.ToString();
    }
}
