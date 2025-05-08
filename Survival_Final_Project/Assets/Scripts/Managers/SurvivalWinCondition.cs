using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SurvivalWinCondition : MonoBehaviour
{
    public GameObject loseCanvas;

    public void PlayerDied()
    {
        LoseGame();
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
}
