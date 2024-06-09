using UnityEngine;

public class MouseFollowLight : MonoBehaviour
{
    private Camera mainCamera;
    private Transform playerTransform;
    private Transform lightTransform;
    private UnityEngine.Rendering.Universal.Light2D spotlight2D;
    private Animator playerAnimator;
    private bool isLightOn = true; // Boolean to control the light's on/off state
    private bool flashlightMode = true; // Flag to determine if we're in flashlight mode
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    private float moveSpeed;

    void Start()
    {
        mainCamera = Camera.main;
        playerTransform = transform.parent;
        lightTransform = GetComponent<Transform>();
        spotlight2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        playerAnimator = playerTransform.GetComponent<Animator>();
        playerMovement = playerTransform.GetComponent<PlayerMovement>(); // Get reference to the PlayerMovement script

        if (spotlight2D == null)
        {
            Debug.LogWarning("No Light2D component found on the GameObject. Please attach a Light2D component.");
        }

        moveSpeed = playerMovement.moveSpeed;
    }

    void Update()
    {
        // Toggle the light on/off when the "F" key is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            isLightOn = !isLightOn;
            if (spotlight2D != null)
            {
                spotlight2D.enabled = isLightOn;
            }
            flashlightMode = isLightOn; // Set flashlight mode based on light state

            // Enable/disable PlayerMovement script based on flashlight mode
            playerMovement.enabled = !flashlightMode;
        }

        if (!flashlightMode)
        {
            // Get player movement input
            Vector2 input = playerMovement.GetMovementInput();

            // If the player is moving
            if (input.magnitude > 0)
            {
                // Rotate the light to face the movement direction
                float angleToMove = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg - 90;
                lightTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToMove));

                // Play walking animation based on cursor direction
                playerAnimator.SetFloat("Horizontal", input.x);
                playerAnimator.SetFloat("Vertical", input.y);
                playerAnimator.SetBool("Moving", true);
            }
            else
            {
                // If the player is not moving, stop the walking animation
                playerAnimator.SetBool("Moving", false);
            }
        }
        else
        {
            playerMovement.enabled = false;
            // If the light is on, rotate the light to face the mouse position
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -mainCamera.transform.position.z; // Ensure proper depth
            Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

            // The light position should be locked to the player
            lightTransform.position = playerTransform.position;

            // Rotate the light to face the mouse position
            Vector2 directionToMouse = worldMousePosition - playerTransform.position;
            float angleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg - 90;
            lightTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToMouse));

            // Set animator parameters for player direction based on mouse position
            playerAnimator.SetFloat("Horizontal", directionToMouse.x);
            playerAnimator.SetFloat("Vertical", directionToMouse.y);

            // Check keyboard input for movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate movement direction based on keyboard input
            Vector2 movementDirection = new Vector2(horizontalInput, verticalInput).normalized;

            // Move the player based on the movement direction and speed
            playerTransform.Translate(movementDirection * moveSpeed * Time.deltaTime);

            // Check if the player is moving
            if (playerMovement.GetMovementInput().magnitude > 0)
            {
                // If the player is moving, set "Moving" parameter to true
                playerAnimator.SetBool("Moving", true);
            }
            else
            {
                // If the player is not moving, set "Moving" parameter to false (idle)
                playerAnimator.SetBool("Moving", false);
            }
        }
    }
    }
