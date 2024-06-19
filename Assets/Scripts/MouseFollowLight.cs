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
    private Flashlight flashlight; // Reference to the Flashlight script

    // Public fields to specify the reference GameObject for the light's position and the flashlight GameObject
    public Transform referenceTransform;
    public GameObject flashlightGameObject;
    public GameObject Light;


    void Start()
    {
        mainCamera = Camera.main;
        playerTransform = transform.parent;
        lightTransform = GetComponent<Transform>();
        spotlight2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        playerAnimator = playerTransform.GetComponent<Animator>();
        playerMovement = playerTransform.GetComponent<PlayerMovement>(); // Get reference to the PlayerMovement script
        flashlight = flashlightGameObject.GetComponent<Flashlight>(); // Get reference to the Flashlight script

        if (spotlight2D == null)
        {
            Debug.LogWarning("No Light2D component found on the GameObject. Please attach a Light2D component.");
        }

        moveSpeed = playerMovement.moveSpeed;

        // Set the initial position of the light to the reference transform's position if provided
        if (referenceTransform != null)
        {
            lightTransform.position = referenceTransform.position;
        }

        // Initially turn off the flashlight
        spotlight2D.enabled = false;
        flashlightGameObject.SetActive(false);
       
    }




    void Update()
    {
        // Toggle the light on/off when the "F" key is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Toggle the flashlight on/off
            flashlight.ToggleLight();

            // Update the flashlight mode based on the light state
            flashlightMode = flashlight.isLightOn;

            // Update the player movement script based on flashlight mode
            playerMovement.enabled = !flashlightMode;

            // Hide or show the flashlight GameObject based on the light state
            flashlightGameObject.SetActive(flashlightMode);

            Light.SetActive(true);
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
            // If the light is on, rotate the light to face the mouse position
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -mainCamera.transform.position.z; // Ensure proper depth
            Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

            // Update the light's position based on the reference transform's position if provided
            if (referenceTransform != null)
            {
                lightTransform.position = referenceTransform.position;
            }

            // Rotate the light to face the mouse position
            Vector2 directionToMouse = worldMousePosition - lightTransform.position;
            float angleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg - 90;
            lightTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToMouse));

            // Set animator parameters for player direction based on mouse position
            playerAnimator.SetFloat("Horizontal", directionToMouse.x);
            playerAnimator.SetFloat("Vertical", directionToMouse.y);

            // Check keyboard input for movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Check if sprint button is pressed
            bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            playerMovement.isSprinting = isSprinting;
            // Calculate movement direction based on keyboard input
            Vector2 movementDirection = new Vector2(horizontalInput, verticalInput).normalized;

            // Determine move speed based on sprinting
            float currentMoveSpeed = isSprinting ? moveSpeed * 1 : moveSpeed;

            // Move the player based on the movement direction and speed
            playerTransform.Translate(movementDirection * currentMoveSpeed * Time.deltaTime);

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
