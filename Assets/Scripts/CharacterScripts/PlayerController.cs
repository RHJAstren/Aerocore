using TMPro;
using UnityEngine;

public enum PlayerState
{
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

    private AudioManager audioManager; // Cache AudioManager reference

    [Header("Player Jumping")]
    public float jumpForce = 10.0f;
    public float gravityMultiplier = 1.0f;

    [Header("Collectable")]
    public int bubbleCount = 0;
    public int bubbleCountFinal = 0;
    public TextMeshProUGUI bubbleCountText;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDist = 0.2f;
    public LayerMask groundLayer;

    [Header("Other")]
    public GameObject PauseMenu;
    public GameObject CompletionMenu;
    public GameObject[] Bubbles;
    public Transform[] SpawnPoints;
    public GameObject enemy;
    private bool isCompleted = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Cache AudioManager reference
        audioManager = FindAnyObjectByType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }

        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        GameManager.gameState = GameState.PLAY;
        Cursor.lockState = CursorLockMode.Locked;
        EnableBubbles();
        Instantiate(enemy, InstantiateEnemy(), Quaternion.identity);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundLayer);

        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1), ForceMode.Acceleration);
        }
    }

    void Update()
    {
        Debug.Log($"Player is in {GameManager.gameState}");
        switch (GameManager.gameState)
        {
            case GameState.PLAY:
                PlayerMovement();
                break;
            case GameState.PAUSED:
                HandlePause();
                break;
            case GameState.COMPLETED:
                Debug.Log($"Player is in {playerState}");
                HandleCompletion();
                break;
            case GameState.GAME_OVER:
                HandleGameOver();
                break;
            default:
                break;
        }
    }

    void EnableBubbles()
    {
        foreach (GameObject bubble in Bubbles)
        {
            bubble.SetActive(true);
        }
    }

    Vector3 InstantiateEnemy()
    {
        int randomIndex = Random.Range(0, SpawnPoints.Length);
        Vector3 spawnPos = SpawnPoints[randomIndex].position;
        return spawnPos;
    }

    void PlayerMovement()
    {
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
        CompletionMenu.SetActive(false);
    }

    void HandlePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * baseSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * baseSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * baseSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * baseSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            baseSpeed = RunSpeed;
        }
        else
        {
            baseSpeed = MovementSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) // Only jump if grounded
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (bubbleCount % 200 == 0 && bubbleCount != 0)
        {
            bubbleCount = 0;
            GameManager.CompleteLevel();
            Debug.Log("Player has won!"); // Log win
            if (audioManager != null)
            {
                audioManager.Stop("GameMusic");
                audioManager.Play("CompletionMusic");
            }
        }
    }

    void HandlePause()
    {
        Cursor.lockState = CursorLockMode.None;
        PauseMenu.SetActive(true);
        audioManager.Pause("GameMusic");
        audioManager.Play("PauseMusic");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.ResumeGame();
            audioManager.Stop("PauseMusic");
            audioManager.Continue("GameMusic");
        }
    }

    void HandleCompletion()
    {
        CompletionMenu.SetActive(true);
        Debug.Log("Level Completed!"); // Log completion
        audioManager.Stop("GameMusic");
        audioManager.Play("CompletionMusic");
    }

    public void AddBubble()
    {
        bubbleCount++;
        bubbleCountFinal++;
        bubbleCountText.text = "Bubbles: " + bubbleCountFinal;
    }

    void Jump()
    {
        if (isGrounded) // Ensure the player is grounded before jumping
        {
            Debug.Log("Player is jumping.");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public override void Attack()
    {
        Debug.Log("Player is attacking.");
    }

    void HandleGameOver()
    {
        playerState = PlayerState.DEAD;
        Debug.Log("Game Over. Player has lost!"); // Log lose
    }
}