using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public Text scoreText;

    public float scoreIncraseRate = 1f; //skor art�� h�z�
    public float initialScore = 0f;
    public Transform playerTransform;
    private float playerStartPosition;
    public const string highScore_key = "HighScore";
    private bool isRunning = true;
    private float highScore;
    private float currentScore;


    public void Awake()
    {
        highScore = PlayerPrefs.GetFloat(highScore_key);
        Debug.Log("skor kaydedildi" + currentScore.ToString());

    }
    public void SetRunningState(bool running)
    {
        isRunning = running;
        if (!isRunning)
        {
            SaveScore(); // Ko�ma durdu�unda skoru kaydet


        }
    }


    public void Update()
    {
        if (isRunning)
        {
            //skoru her saniyede artt�rmak i�in
            currentScore = playerTransform.position.z - playerStartPosition;
            currentScore += currentScore * scoreIncraseRate * Time.deltaTime;
            Debug.Log("score:" + currentScore.ToString());

            UpdateScore();
        }


    }

    public void UpdateScore()
    {

        if (currentScore >= highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetFloat(highScore_key, highScore);
            PlayerPrefs.Save();
            Debug.Log("En y�ksek skor g�ncellendi: " + highScore.ToString());

            ChangeScoreColor();



        }


        //skoru g�ster
        scoreText.text = "Score: " + Mathf.Round(currentScore).ToString();
    }

    public void IncreaseScore(int amount)
    {
        currentScore += amount;
        UpdateScore();

    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScore();

    }
    public void SaveScore()
    {
        PlayerPrefs.SetFloat(highScore_key, currentScore);
        PlayerPrefs.Save();
    }

    private void ChangeScoreColor()
    {
        if (scoreText != null)
        {

            scoreText.color = Color.red;


        }
    }

}

