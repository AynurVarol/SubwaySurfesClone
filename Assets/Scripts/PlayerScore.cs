using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public Text scoreText;
   
    public float initialScore = 0f;
    public const string highScore_key = "HighScore";
    [SerializeField] public float scoreIncraseRate = 1f; //skor artýþ hýzý
    public Transform playerTransform;

    private bool isRunning = true;
    private float playerStartPosition;
    private float _highScore;
    private float _currentScore;


    public void Awake()
    {
        _highScore = PlayerPrefs.GetFloat(highScore_key);
        Debug.Log("skor kaydedildi" + _currentScore.ToString());

        // Baþlangýç pozisyonunu ayarla
        playerStartPosition = playerTransform.position.z;
        // Baþlangýç skorunu sýfýr yap
        _currentScore = 0f;
        // Skoru güncelle
        UpdateScore();

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
           _currentScore = playerTransform.position.z - playerStartPosition;
            _currentScore += _currentScore * scoreIncraseRate * Time.deltaTime;
            //Debug.Log("score:" + _currentScore.ToString());

            UpdateScore();
        }


    }

    public void UpdateScore()
    {

        if (_currentScore >= _highScore)
        {
            _highScore = _currentScore;
            PlayerPrefs.SetFloat(highScore_key, _highScore);
            PlayerPrefs.Save();
           // Debug.Log("En yüksek skor güncellendi: " + _highScore.ToString());

            ChangeScoreColor();



        }


        //skoru göster
        scoreText.text = "Score: " + Mathf.Round(_currentScore).ToString();
    }


    public void IncreaseScore(int amount)
    {
        _currentScore += amount;
        UpdateScore();

    }


    public void ResetScore()
    {
        _currentScore = 0;
        UpdateScore();

    }

    public void SaveScore()
    {
        PlayerPrefs.SetFloat(highScore_key, _currentScore);
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

