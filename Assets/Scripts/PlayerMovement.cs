using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f; // Normal speed of the player movement
    public float sprintSpeed = 6f; // Speed of the player movement when sprinting
    public float maxStamina = 100f; // Maximum stamina value
    public float staminaRegenRate = 10f; // Rate at which stamina regenerates per second
    public float sprintStaminaCost = 30f; // Stamina cost of sprinting
    

    private float currentStamina; // Current stamina value
    private bool isSprinting; // Flag indicating whether the player is currently sprinting
    private bool canRegenStamina = true; // Flag indicating whether stamina can regenerate
    private float sprintCooldownTimer; // Timer for sprint cooldown
    private float staminaRegenCooldownTimer; // Timer for stamina regeneration cooldown
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    // Joystick references
    public Joystick joystick; // Reference to the joystick
    public Button sprintButton; // Reference to the sprint button

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina; // Set initial stamina value to max

        // Register button events
        if (sprintButton != null)
        {
            EventTrigger trigger = sprintButton.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((data) => { StartSprinting(); });
            trigger.triggers.Add(pointerDownEntry);

            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            pointerUpEntry.eventID = EventTriggerType.PointerUp;
            pointerUpEntry.callback.AddListener((data) => { StopSprinting(); });
            trigger.triggers.Add(pointerUpEntry);
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
        if (!canRegenStamina && currentStamina > 0) // Adjusted condition
        {
            staminaRegenCooldownTimer -= Time.deltaTime;
            if (staminaRegenCooldownTimer <= 0)
            {
                canRegenStamina = true;
            }
        }

        // Check if the player is sprinting
        if (keyboardInput.magnitude > 0 && Input.GetKey(KeyCode.LeftShift) && currentStamina >= sprintStaminaCost * Time.fixedDeltaTime && sprintCooldownTimer <= 0)
        {
            StartSprinting();
        }

        // Stop sprinting if the player is not moving or doesn't have enough stamina or cooldown is active
        if (!isMoving || currentStamina < sprintStaminaCost * Time.fixedDeltaTime || sprintCooldownTimer > 0)
        {
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
    // Update stamina based on sprinting state
    // Update stamina based on sprinting state
    void UpdateStamina()
    {
        if (isSprinting)
        {
            // Decrease stamina when sprinting
            currentStamina = Mathf.Max(0f, currentStamina - sprintStaminaCost * Time.fixedDeltaTime);

            // Start the stamina regeneration cooldown when stamina reaches zero
            if (currentStamina <= 0 && canRegenStamina)
            {
                canRegenStamina = false;
                staminaRegenCooldownTimer = 3f; // Set the cooldown timer to 3 seconds
            }
        }
        else if (canRegenStamina && !isSprinting)
        {
            // Only regenerate stamina if it's not already at zero
            if (currentStamina > 0)
            {
                // Increase stamina gradually when not sprinting, and sprint button is not pressed, and can regenerate stamina
                currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenRate * Time.fixedDeltaTime);
            }
        }

        // Update the stamina regeneration cooldown timer
        if (!canRegenStamina)
        {
            staminaRegenCooldownTimer -= Time.deltaTime;
            if (staminaRegenCooldownTimer <= 0)
            {
                canRegenStamina = true;
            }
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
