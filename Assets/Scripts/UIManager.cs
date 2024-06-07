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

        // Initially hide stamina UI elements
        SetStaminaUIVisible(true);
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
            staminaText.text = "Stamina: " + playerMovement.GetCurrentStamina().ToString("F0") + "%";
        }
    }

    public void SetStaminaUIVisible(bool visible)
    {
        staminaSlider.gameObject.SetActive(visible);
        staminaText.gameObject.SetActive(visible);
    }

    public void HideStaminaUI()
    {
        // Hide stamina UI elements
        SetStaminaUIVisible(false);
    }
}
