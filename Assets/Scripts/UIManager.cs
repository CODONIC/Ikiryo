using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Slider staminaSlider;
    public TextMeshProUGUI staminaText; // Reference to the TextMeshPro component
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Set slider value to the maximum stamina value
        if (playerMovement != null)
        {
            staminaSlider.value = playerMovement.maxStamina;
        }
    }

    void Update()
    {
        if (playerMovement != null)
        {
            // Update slider value based on player's current stamina
            staminaSlider.value = playerMovement.GetCurrentStamina();

            // Update stamina text
            UpdateStaminaText();
        }
    }

    void UpdateStaminaText()
    {
        if (staminaText != null && playerMovement != null)
        {
            // Display current stamina value in the TextMeshPro component
            staminaText.text = "Stamina: " + playerMovement.GetCurrentStamina().ToString("F0");
        }
    }
}
