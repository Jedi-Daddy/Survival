using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SurvivalWinCondition : MonoBehaviour
{
    [Header("Survival Settings")]
    public int daysToSurvive = 10;
    public float dayDuration = 60f; // Время в секундах для одного дня
    public Text dayCounterText;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject deathMenuCanvas;

    private int currentDay = 1;
    private float dayTimer;
    private bool gameOver = false;

    void Start()
    {
        UpdateDayCounter();
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
        if (deathMenuCanvas != null) deathMenuCanvas.SetActive(false);
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
            dayCounterText.text = "Day: " + currentDay;
        }
    }

    public void PlayerDied()
    {
        LoseGame();
    }

    void WinGame()
    {
        gameOver = true;
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Debug.Log("Victory screen shown.");
        }
        else
        {
            Debug.LogError("Win screen not assigned!");
        }
        Time.timeScale = 0f;
        Debug.Log("Victory! You survived " + daysToSurvive + " days.");
    }

    public void LoseGame()
    {
        gameOver = true;
        if (loseScreen != null)
        {
            loseScreen.SetActive(true);
            Debug.Log("Lose screen shown.");
        }
        else
        {
            Debug.LogError("Lose screen not assigned!");
        }
        if (deathMenuCanvas != null)
        {
            deathMenuCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        Time.timeScale = 0f;
        Debug.Log("Game Over! You didn't survive.");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed.");
    }
}
