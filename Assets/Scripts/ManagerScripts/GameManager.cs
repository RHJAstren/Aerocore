using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MENU = 100,
    PLAY = 101,
    PAUSED = 102,
    GAME_OVER = 103,
    COMPLETED = 104
}

public class GameManager : Singleton<GameManager>
{
    public static GameState gameState = GameState.MENU;
    private AudioManager audioManager;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Cache AudioManager reference
        audioManager = FindAnyObjectByType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
            return;
        }

        // Play main menu music
        audioManager.Play("MainMenu");
    }

    public void StartGame()
    {
        // Transition to PLAY state
        gameState = GameState.PLAY;

        // Stop main menu music and play in-game music
        if (audioManager != null)
        {
            audioManager.Stop("MainMenu");
            audioManager.Play("InGame");
        }

        // Change to gameplay scene
        ChangeScene(true);
    }

    public static void PauseGame()
    {
        // Transition to PAUSED state
        gameState = GameState.PAUSED;

        // Pause in-game music and play pause menu music
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Pause("InGame");
            AudioManager.instance.Play("PauseMenu");
        }

        // Freeze game time
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        // Transition back to PLAY state
        gameState = GameState.PLAY;

        // Stop pause menu music and resume in-game music
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Stop("PauseMenu");
            AudioManager.instance.Continue("InGame");
        }

        // Resume game time
        Time.timeScale = 1;
    }

    public static void CompleteLevel()
    {
        // Transition to COMPLETED state
        gameState = GameState.COMPLETED;

        // Stop in-game music and play completion music
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Stop("InGame");
            AudioManager.instance.Play("Completion");
        }

        // Freeze game time
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        // Transition to GAME_OVER state
        gameState = GameState.GAME_OVER;

        // Stop in-game music and play game over music
        if (audioManager != null)
        {
            audioManager.Stop("InGame");
            audioManager.Play("GameOver");
        }
    }

    public static void Reload()
    {
        // Reset to PLAY state and reload current scene
        gameState = GameState.PLAY;

        // Stop pause menu music if it's playing
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Stop("PauseMenu");
        }

        // Resume time scale and reload the current scene
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Resume in-game music
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("InGame");
        }
    }

    public void QuitGame()
    {
        // Exit the application
        Application.Quit();
    }

    public static void ChangeScene(bool isAddition)
    {
        // Determine next or previous scene
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = isAddition ? currentScene + 1 : currentScene - 1;

        // Stop current music to prevent overlap
        if (AudioManager.instance != null)
        {
            if (gameState == GameState.MENU)
            {
                AudioManager.instance.Stop("MainMenu");
            }
            else if (gameState == GameState.PLAY)
            {
                AudioManager.instance.Stop("InGame");
            }
        }

        // Load the new scene
        SceneManager.LoadScene(nextScene);

        // Play appropriate music for the new scene
        if (AudioManager.instance != null)
        {
            if (isAddition)
            {
                AudioManager.instance.Play("InGame");
            }
            else
            {
                AudioManager.instance.Play("MainMenu");
            }
        }
    }
}
