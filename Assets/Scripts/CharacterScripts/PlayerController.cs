using System;
using TMPro;
using UnityEngine;

public enum PlayerState{
    NORMAL,
    DEAD
}

public class PlayerController : Character
{
    private PlayerState playerState = PlayerState.NORMAL;

    [Header ("Player Jumping")]
    public float jumpForce = 10.0f;
    public float gravityMultiplier = 1.0f;

    public int bubbleCount = 0;
    public TextMeshProUGUI bubbleCountText;

    public GameObject PauseMenu;

    float baseSpeed = 0.0f;

    public static PlayerController instance;

    public event Action BubbleCollected;

    void Update(){
        switch (GameManager.gameState) {
            case GameState.PLAY:
                PlayerMovement();
                break;
            case GameState.PAUSED:
                HandlePause();
                break;
            case GameState.GAME_OVER:
                HandleGameOver();
                break;
            default:
                break;
        }
    }
    
    void PlayerMovement(){
        switch (playerState)
        {
            case PlayerState.NORMAL:
                Debug.Log($"Player is in {playerState}");
                HandlePlayer();
                break;
            case PlayerState.DEAD:
                Debug.Log($"Player is in {playerState}");
                GameManager.gameState = GameState.GAME_OVER;
                break;
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        PauseMenu.SetActive(false);
    }

    void HandlePlayer(){

        if (Input.GetKey(KeyCode.W)){
            transform.Translate(Vector3.forward * baseSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)){
            transform.Translate(Vector3.back * baseSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)){
            transform.Translate(Vector3.left * baseSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)){
            transform.Translate(Vector3.right * baseSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift)){
            baseSpeed = RunSpeed;
        }
        else{
            baseSpeed = MovementSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Escape)){
            GameManager.PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }

        if (Input.GetMouseButtonDown(0)){
            Attack();
        }
    }

    void HandlePause(){
        Cursor.lockState = CursorLockMode.None;
        PauseMenu.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.gameState == GameState.PAUSED){
            GameManager.gameState = GameState.PLAY;
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Q)){
            GameManager.gameState = GameState.MENU;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None;
            GameManager.ChangeScene(false);
        }
    }

    public void AddBubble(){
        BubbleCollected.Invoke();
        bubbleCount++;
        bubbleCountText.text = "Bubbles: " + bubbleCount;
    }

    void Jump(){
        Debug.Log("Player is jumping.");
    }

    public override void Attack(){
        Debug.Log("Player is attacking.");
    }
    
    void HandleGameOver(){
        playerState = PlayerState.DEAD;
        Debug.Log("Game Over.");
    }
}
