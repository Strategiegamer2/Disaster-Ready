using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables for movement
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 4f;
    public float gravity = -9.81f;

    // Camera settings
    public Transform playerCamera;
    private float cameraPitch = 0f;
    [SerializeField] private float mouseSensitivity = 100f;

    // Character Controller
    private CharacterController controller;

    // Variables to handle velocity and jumping
    private Vector3 velocity;
    private bool isGrounded;

    // Reference to player input
    private float moveX;
    private float moveZ;

    public float MouseSensitivity
    {
        get { return mouseSensitivity; }
        set { mouseSensitivity = Mathf.Clamp(value, 100f, 300f); } // Ensure it's within desired range
    }

    void Start()
    {
        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the CharacterController attached to the player
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        // Check if the player is on the ground
        isGrounded = controller.isGrounded;

        // Reset downward velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get player input for movement
        moveX = Input.GetAxis("Horizontal"); // A, D or Arrow keys (strafe)
        moveZ = Input.GetAxis("Vertical");   // W, S or Arrow keys (forward, backward)

        // Handle sprinting
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        // Move the player based on input
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jump logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 5 * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 5 * Time.deltaTime;

        // Adjust the vertical angle of the camera (pitch)
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f); // Limit the up/down look to avoid flipping over

        // Rotate the camera based on pitch
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        // Rotate the player based on horizontal mouse movement
        transform.Rotate(Vector3.up * mouseX);
    }
}
