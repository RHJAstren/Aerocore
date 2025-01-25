using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MENU = 100,
    PLAY = 101,
    PAUSED = 102,
    GAME_OVER = 103
}

public class GameManager : Singleton<GameManager>
{
    public static GameState gameState = GameState.MENU;

    void Update(){
        Debug.Log($"Game State in Update: {gameState}");
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame(){
        gameState = GameState.PLAY;
        ChangeScene(true);
    }

    public static void PauseGame(){
        gameState = GameState.PAUSED;
        Time.timeScale = 0;
    }

    public void GameOver(){
        gameState = GameState.GAME_OVER;
    }

    public void Reload(){
        gameState = GameState.PLAY;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public static void ChangeScene(bool isAddition){
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = isAddition ? currentScene + 1 : currentScene - 1;
        SceneManager.LoadScene(nextScene);
    }
}
