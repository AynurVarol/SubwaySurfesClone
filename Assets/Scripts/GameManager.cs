using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    /// <summary>
///  oyuncunun can� bitince gelecek gameover ekran�
///  oyunu tekrar ba�latmak ve oyundan tamamen ��kmak i�in gerekli kodlar
/// </summary>
/// 
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static int numberOfCoins;
    public Text coinsText;
    public Text scoreText;
    private PlayerScore playerScore;

   
    void Start()
    {
        gameOver = false;
        //playerScore = FindObjectOfType<PlayerScore>();
        playerScore = GetComponent<PlayerScore>();
        numberOfCoins = 0;

        //Oyun ba�lad���nda skoru s�f�rla
        if (playerScore != null)
        {
            playerScore.ResetScore();
        }



    }

   
    public void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);

            if(playerScore != null)
            {
                playerScore.SetRunningState(false);
            }

        }

        coinsText.text = " " + numberOfCoins;

        if (playerScore != null && !gameOver)
        {
            playerScore.Update();
            playerScore.UpdateScore();
        }

        


    }

    public void RestartButton()
    {
        
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;

        gameOver = false;

        gameOverPanel.SetActive(false);


        if (playerScore != null)
        {
            //skoru s�f�rlamak i�in;
            playerScore.ResetScore();
            Debug.Log("skor s�f�rlanma koduna girdi");

            playerScore.SetRunningState(true);

        }
        



    }


    public void ExitButton()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

}
