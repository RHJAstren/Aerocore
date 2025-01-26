using TMPro;
using UnityEngine;

public enum PlayerState{
    NORMAL,
    DEAD
}

public class PlayerController : Character
{
    private PlayerState playerState = PlayerState.NORMAL;
    float baseSpeed = 0.0f;
    public static PlayerController instance;
    private Rigidbody rb;
    public bool isGrounded = true;

    [Header ("Player Jumping")]
    public float jumpForce = 10.0f;
    public float gravityMultiplier = 1.0f;

    [Header ("Collectable")]
    public int bubbleCount = 0;
    public TextMeshProUGUI bubbleCountText;

    [Header ("Ground Check")]
    public Transform groundCheck;
    public float groundDist = 0.2f;
    public LayerMask groundLayer;

    [Header ("Other")]
    public GameObject PauseMenu;
    
    void Awake(){
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start(){
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        GameManager.gameState = GameState.PLAY;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate(){
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundLayer);
        
        if (!isGrounded){
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1), ForceMode.Acceleration);
        }
    }

    void Update(){
        Debug.Log($"Player is in {GameManager.gameState}");
        switch (GameManager.gameState) {
            case GameState.PLAY:
                if (isGrounded){
                    PlayerMovement();
                }
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
        bubbleCount++;
        bubbleCountText.text = "Bubbles: " + bubbleCount;
    }

    void Jump(){
        Debug.Log("Player is jumping.");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public override void Attack(){
        Debug.Log("Player is attacking.");
    }
    
    void HandleGameOver(){
        playerState = PlayerState.DEAD;
        Debug.Log("Game Over.");
    }
}
