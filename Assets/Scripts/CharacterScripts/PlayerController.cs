using TMPro;
using UnityEngine;

public enum PlayerState{
    NORMAL,
    DEAD
}

public class PlayerController : Character
{
    private PlayerState playerState = PlayerState.NORMAL;

    public int bubbleCount = 0;
    public TextMeshProUGUI bubbleCountText;

    void Update(){ 
        switch(GameManager.gameState){
            case GameState.PLAY:
                PlayerMovement();
                break;
            case GameState.PAUSED:
                HandlePause();
                break;
            case GameState.GAME_OVER:
                HandleGameOver();
                break;
        }
    }
    
    void PlayerMovement(){
        switch (playerState){
            case PlayerState.NORMAL:
                HandlePlayer();
                break;
            case PlayerState.DEAD:
                GameManager.gameState = GameState.GAME_OVER;
                break;
        }
    }

    void HandlePlayer(){
        if (Input.GetKey(KeyCode.W)){
            transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)){
            transform.Translate(Vector3.back * MovementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)){
            transform.Translate(Vector3.left * MovementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)){
            transform.Translate(Vector3.right * MovementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift)){
            MovementSpeed = 12.5f;
        }
        else{
            MovementSpeed = 7.5f;
        }
        if (Input.GetKey(KeyCode.Escape)){
            GameManager.instance.PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }
        if (Input.GetMouseButtonDown(0)){
            Attack();
        }
    }

    void Jump(){
        Debug.Log("Player is jumping.");
    }

    public override void Attack(){
        Debug.Log("Player is attacking.");
    }

    void HandlePause(){
        Debug.Log("Game is paused.");
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.gameState == GameState.PAUSED){
            GameManager.gameState = GameState.PLAY;
            Time.timeScale = 1;
        }
    }
    
    void HandleGameOver(){
        playerState = PlayerState.DEAD;
        Debug.Log("Game Over.");
    }
}
