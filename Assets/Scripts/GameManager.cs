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
    public static int numberOfCoins;

    public Text coinsText;
    public Text scoreText;
    public GameObject gameOverPanel;
    public GameObject startCanvas;
    public Button startButton;

    private PlayerScore playerScore;

   
    void Start()
    { 
        // Oyun ba�lad���nda Start panelini g�ster
        startCanvas.SetActive(true);

        Time.timeScale = 0;


        // Start butonuna t�klama 
        startButton.onClick.AddListener(StartGame);
        gameOver = false;
       
        playerScore = GetComponent<PlayerScore>();
        numberOfCoins = 0;

        gameOverPanel.SetActive(false);
       
       
        //Oyun ba�lad���nda skoru s�f�rla
        if (playerScore != null)
        {
            playerScore.ResetScore();
        }



    }

    void StartGame()
    {
    
        Debug.Log("Oyun ba�lat�l�yor!");

        // Oyun ba�lad���nda Start panelini kapat
        startCanvas.SetActive(false);
        Time.timeScale = 1;
        
       
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
