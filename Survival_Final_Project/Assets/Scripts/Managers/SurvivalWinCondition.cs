using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SurvivalWinCondition : MonoBehaviour
{
    [Header("Survival Settings")]
    public int daysToSurvive = 3;          
    public float dayDuration = 400f;        
    public Text dayCounterText;            
    public GameObject winCanvas;           
    public GameObject loseCanvas;         

    private int currentDay = 0;            
    private float dayTimer;                
    private bool gameOver = false;         

    void Start()
    {
        currentDay = 0;
        dayTimer = 0f;
        gameOver = false;

        UpdateDayCounter();  

        if (winCanvas != null) winCanvas.SetActive(false);
        if (loseCanvas != null) loseCanvas.SetActive(false);
    }


    void Update()
    {
        if (gameOver) return;

        
        dayTimer += Time.deltaTime;

        if (dayTimer >= dayDuration)
        {
            dayTimer = 0f;
            currentDay++;
            UpdateDayCounter();

            
            if (currentDay > daysToSurvive)
            {
                WinGame();
            }
        }
    }

    
    void UpdateDayCounter()
    {
        if (dayCounterText != null)
        {
            dayCounterText.text = "Day: " + currentDay + " / " + daysToSurvive;
        }
        else
        {
            Debug.LogError("Day counter text not assigned!");
        }
    }

    public void PlayerDied()
    {
        LoseGame();
    }

    void WinGame()
    {
        gameOver = true;
        if (winCanvas != null)
        {
            winCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Victory! You survived " + daysToSurvive + " days.");
        }
        else
        {
            Debug.LogError("Win canvas not assigned!");
        }
        Time.timeScale = 0f;
    }


    public void LoseGame()
    {
        if (loseCanvas != null)
        {
            loseCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Game Over! You couldn't survive on the island.");
        }
        Time.timeScale = 0f;
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        gameOver = false;  
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed.");
    }
}
