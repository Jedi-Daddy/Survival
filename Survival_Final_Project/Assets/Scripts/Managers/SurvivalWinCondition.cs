using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SurvivalWinCondition : MonoBehaviour
{
    [Header("Survival Settings")]
    public int daysToSurvive = 3;          // Количество дней, которые нужно выжить
    public float dayDuration = 60f;        // Длительность одного дня в секундах
    public Text dayCounterText;            // Текстовый объект для отображения дней
    public GameObject winCanvas;           // Канвас победы
    public GameObject loseCanvas;          // Канвас поражения

    private int currentDay = 1;            // Текущий день
    private float dayTimer;                // Таймер для отсчёта времени
    private bool gameOver = false;         // Флаг завершения игры

    void Start()
    {
        currentDay = 1;
        dayTimer = 0f;
        gameOver = false;

        UpdateDayCounter();  // Обновляем счётчик сразу при запуске

        if (winCanvas != null) winCanvas.SetActive(false);
        if (loseCanvas != null) loseCanvas.SetActive(false);
    }


    void Update()
    {
        if (gameOver) return;

        // Отсчитываем время до следующего дня
        dayTimer += Time.deltaTime;

        if (dayTimer >= dayDuration)
        {
            dayTimer = 0f;
            currentDay++;
            UpdateDayCounter();

            // Проверяем условие победы
            if (currentDay > daysToSurvive)
            {
                WinGame();
            }
        }
    }

    // Обновляем текстовый счётчик на канвасе
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

    // Перезапуск игры
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Выход в главное меню
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Завершение игры
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed.");
    }
}
