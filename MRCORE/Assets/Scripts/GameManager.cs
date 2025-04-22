using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameDuration = 120f; // 2 minutes game time
    public int targetAnimalsSaved = 5; // How many animals need to be saved to win

    [Header("Tags and References")]
    public string animalTag = "Animal"; // Make sure this is consistent!
    public GameObject portal;

    [Header("UI Elements")]
    public TextMeshProUGUI animalCounterText;
    public TextMeshProUGUI timerText;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject gameOverPanel;

    // Internal variables
    private float timeRemaining;
    private int animalsSaved = 0;
    private bool gameEnded = false;

    void Start()
    {
        // Initialize game
        timeRemaining = gameDuration;
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        UpdateAnimalCounter();
    }

    void Update()
    {
        if (gameEnded) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            EndGame(false);
        }

        UpdateAnimalCounter();
    }

    public void AnimalSaved()
    {
        animalsSaved++;

        // Check win condition
        if (animalsSaved >= targetAnimalsSaved)
        {
            EndGame(true);
        }

        // Update UI
        UpdateAnimalCounter();
    }

    public void AnimalEaten()
    {
        // Show lose message temporarily
        StartCoroutine(ShowLoseMessage());
    }

    IEnumerator ShowLoseMessage()
    {
        losePanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        losePanel.SetActive(false);
    }

    void UpdateAnimalCounter()
    {
        int animalCount = GameObject.FindGameObjectsWithTag(animalTag).Length;

        if (animalCounterText != null)
        {
            animalCounterText.text = $"Animals: {animalCount} | Saved: {animalsSaved}";
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    void EndGame(bool isWin)
    {
        gameEnded = true;

        if (isWin)
        {
            winPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}