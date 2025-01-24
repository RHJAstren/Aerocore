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
    public GameObject PauseMenu;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame(){
        gameState = GameState.PLAY;
        StartCoroutine(LoadAsync(1));
    }

    public void PauseGame(){
        gameState = GameState.PAUSED;
        PauseMenu.SetActive(true);
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

    IEnumerator LoadAsync(int sceneIndex) {
        gameState = GameState.PLAY;
        Debug.Log("Game has started.");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Loading progress: " + progress * 100 + "%");
            yield return null;
        }
    }
}
