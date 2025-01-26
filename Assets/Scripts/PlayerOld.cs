using UnityEngine;

public class Playerold : MonoBehaviour
{
    public float walkSpeed = 5f;        // Walking speed
    public float sprintSpeed = 10f;    // Sprinting speed
    public float jumpForce = 10f;      // Jump force
    public float gravityMultiplier = 1f; // Custom gravity multiplier for more control

    private Rigidbody rb;
    private bool isGrounded;

    public Transform groundCheck;       // Empty GameObject to check if grounded
    public float groundDistance = 0.2f; // Ground check radius
    public LayerMask groundMask;        // Layers considered as ground

    public Transform playerCamera;      // The camera attached to the player
    private float xRotation = 0f;       // Vertical camera rotation
    public float mouseSensitivity = 100f; // Mouse sensitivity

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Avoid tunneling
    }

    private void FixedUpdate()
    {
        // Ground check: Check for overlaps with the ground mask
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Apply custom gravity if not grounded
        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1), ForceMode.Acceleration);
        }
    }

    private void Update()
    {
        // Mouse input for looking around
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically (clamping to avoid flipping)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Prevent mid-air movement
        if (isGrounded)
        {
            HandleMovement();
        }

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleMovement()
    {
        // Movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Calculate speed
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        // Apply movement
        Vector3 velocity = move * speed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }
}
