using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{  
    /// <summary>
///  oyuncunun caný bitince gelecek gameover ekraný
///  oyunu tekrar baþlatmak ve oyundan tamamen çýkmak için gerekli kodlar
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
        // Oyun baþladýðýnda Start panelini göster
        startCanvas.SetActive(true);

        Time.timeScale = 0;


        // Start butonuna týklama 
        startButton.onClick.AddListener(StartGame);
        gameOver = false;
       
        playerScore = GetComponent<PlayerScore>();
        numberOfCoins = 0;

        gameOverPanel.SetActive(false);
       
       
        //Oyun baþladýðýnda skoru sýfýrla
        if (playerScore != null)
        {
            playerScore.ResetScore();
        }



    }

    void StartGame()
    {
    
        Debug.Log("Oyun baþlatýlýyor!");

        // Oyun baþladýðýnda Start panelini kapat
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
            //skoru sýfýrlamak için;
            playerScore.ResetScore();
            Debug.Log("skor sýfýrlanma koduna girdi");

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
