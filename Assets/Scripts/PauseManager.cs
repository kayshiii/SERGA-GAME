using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public GameObject pauseMenu;
    public Button resumeButton;
    public Button quitButton;

    private bool isPaused = false;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        instance = this;
    }

    private void Start()
    {
        // Hide pause menu at start
        pauseMenu.SetActive(false);
        
        // Add button listeners
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(ReturnGame);
    }

    private void Update()
    {
        // Toggle pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // Show cursor
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // Hide cursor
        Cursor.visible = false;
        isPaused = false;
    }

    public void ReturnGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
} 