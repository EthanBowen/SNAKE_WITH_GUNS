using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class _GameController : MonoBehaviour
{
    public GameObject gameOverUI;
    [SerializeField]
    private TextMeshProUGUI gameoverScoreText;

    [SerializeField]
    private AudioSource AS;
    [SerializeField]
    private AudioClip PlayingMusic;
    [SerializeField]
    private AudioClip GameoverMusic;

    [SerializeField]
    private GameObject ScoreUI;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public int currentScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        if (AS != null)
        {
            AS.clip = PlayingMusic;
            AS.Play();
        }

        Cursor.visible = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    public void ModifyScore(int points)
    {
        currentScore += points;
    }

    void UpdateScore()
    {
        scoreText.text = "SCORE: " + currentScore.ToString();
    }

    public void GameOver()
    {
        Cursor.visible = true;
        if( gameOverUI != null )
        {
            gameOverUI.SetActive( true );
            if(gameoverScoreText != null )
            {
                gameoverScoreText.text = "FINAL SCORE: " + currentScore.ToString();
            }
        }
        if(ScoreUI != null )
        {
            ScoreUI.SetActive(false);
        }

        if (AS != null)
        {
            AS.clip = GameoverMusic;
            AS.Play();
        }

    }
}
