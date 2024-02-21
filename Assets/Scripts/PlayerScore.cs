using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public Text scoreText;

    public float scoreIncraseRate = 1f; //skor artýþ hýzý
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
            SaveScore(); // Koþma durduðunda skoru kaydet


        }
    }


    public void Update()
    {
        if (isRunning)
        {
            //skoru her saniyede arttýrmak için
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
            Debug.Log("En yüksek skor güncellendi: " + highScore.ToString());

            ChangeScoreColor();



        }


        //skoru göster
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

