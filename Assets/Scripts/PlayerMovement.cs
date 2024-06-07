using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f; // Normal speed of the player movement
    public float sprintSpeed = 6f; // Speed of the player movement when sprinting
    public float maxStamina = 100f; // Maximum stamina value
    public float staminaRegenRate = 10f; // Rate at which stamina regenerates per second
    public float sprintStaminaCost = 30f; // Stamina cost of sprinting

    private float currentStamina; // Current stamina value
    public bool isSprinting; // Flag indicating whether the player is currently sprinting
    private bool canRegenStamina = true; // Flag indicating whether stamina can regenerate
    private float sprintCooldownTimer; // Timer for sprint cooldown
    private float staminaRegenCooldownTimer; // Timer for stamina regeneration cooldown
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    // Joystick references
    public Joystick joystick; // Reference to the joystick
    public Button sprintButton; // Reference to the sprint button

    private UIManager uiManager; // Reference to the UIManager
    private bool staminaUIVisible; // Flag to track if the stamina UI is visible
    private bool shiftHeld; // Flag to track if the shift key is held down

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina; // Set initial stamina value to max

        uiManager = FindObjectOfType<UIManager>(); // Get reference to UIManager
        staminaUIVisible = false; // Initially the stamina UI is not visible
        shiftHeld = false; // Initially the shift key is not held down

        // Register button events
        if (sprintButton != null)
        {
            sprintButton.onClick.AddListener(() => {
                if (isSprinting)
                {
                    StopSprinting();
                }
                else
                {
                    StartSprinting();
                }
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from joystick or keyboard
        Vector2 joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        Vector2 keyboardInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Prioritize keyboard input if any key is pressed, otherwise use joystick input
        movement = keyboardInput.magnitude > 0 ? keyboardInput : joystickInput;

        // Normalize the movement vector to prevent faster diagonal movement
        movement = movement.normalized;

        // Check if the player is moving
        bool isMoving = movement.magnitude > 0;

        // Update animation based on movement
        UpdateAnimation(movement);

        // Update sprint cooldown timer
        if (sprintCooldownTimer > 0)
        {
            sprintCooldownTimer -= Time.deltaTime;
        }

        // Update stamina regeneration cooldown timer
        if (!canRegenStamina)
        {
            staminaRegenCooldownTimer -= Time.deltaTime;
            if (staminaRegenCooldownTimer <= 0)
            {
                canRegenStamina = true;
            }
        }

        // Check if the player should start or stop sprinting based on keyboard input
        if (Input.GetKey(KeyCode.LeftShift) && isMoving && currentStamina > 0 && sprintCooldownTimer <= 0)
        {
            shiftHeld = true;
            StartSprinting();
        }
        else
        {
            // Stop sprinting if either shift key is released or stamina is depleted
            shiftHeld = false;
            StopSprinting();
        }
    }


    // FixedUpdate is called at a fixed interval and is used for physics calculations
    void FixedUpdate()
    {
        // Calculate current speed based on sprinting state
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // Move the player
        MoveCharacter(movement, currentSpeed);

        // Update stamina
        UpdateStamina();
    }

    // Move the player based on input and speed
    void MoveCharacter(Vector2 input, float speed)
    {
        // Normalize the input vector to prevent faster diagonal movement
        input = input.normalized;

        // Calculate the movement vector with the adjusted speed
        Vector2 movement = input * speed * Time.fixedDeltaTime;

        // Move the player only if there is movement input
        if (input.magnitude > 0)
        {
            rb.MovePosition(rb.position + movement);
        }
    }

    // Update animation parameters based on input
    void UpdateAnimation(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            animator.SetFloat("Horizontal", input.x);
            animator.SetFloat("Vertical", input.y);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    // Update stamina based on sprinting state
    void UpdateStamina()
    {
        if (isSprinting)
        {
            // Decrease stamina when sprinting
            currentStamina = Mathf.Max(0f, currentStamina - sprintStaminaCost * Time.fixedDeltaTime);

            // Start the stamina regeneration cooldown every time stamina is reduced
            canRegenStamina = false;
            staminaRegenCooldownTimer = 2f; // Set the cooldown timer to 2 seconds
        }
        else if (canRegenStamina)
        {
            // Regenerate stamina when not sprinting and allowed to regenerate
            currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenRate * Time.fixedDeltaTime);
        }
    }

    void StartSprinting()
    {
        isSprinting = true;
       
    }

    void StopSprinting()
    {
        isSprinting = false;
       
    }

   
}
