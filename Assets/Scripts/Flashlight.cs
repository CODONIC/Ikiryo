using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class Flashlight : MonoBehaviour
{
    public GameObject lightObject; // Reference to the GameObject containing the 2D light
    public float power = 100f; // Initial power level
    public float powerDecayRate = 5f; // Rate at which power decreases per second when light is on
    public float powerRegenRate = 10f; // Rate at which power regenerates per second when light is off

    public bool isLightOn = false;
    private Light2D lightComponent;

    // Reference to the UI Slider
    public Slider powerSlider;

    // Reference to the TextMeshPro text
    public TextMeshProUGUI powerText;

    void Start()
    {
        lightComponent = lightObject.GetComponent<Light2D>();
        UpdatePowerUI();
    }

    void Update()
    {
        if (isLightOn)
        {
            // Calculate the decay rate per second to make it last for 5 minutes
            float decayRatePerSecond = power / (3 * 60); // 5 minutes in seconds

            // If the light is on, decrement power over time
            if (power > 0)
            {
                power -= decayRatePerSecond * Time.deltaTime;
            }
            else
            {
                // If power reaches zero, turn off the light
                lightComponent.enabled = false;
                isLightOn = false;
            }
        }

        // Clamp power to be between 0 and 100
        power = Mathf.Clamp(power, 0f, 100f);

        // Update the UI Slider and TextMeshPro text
        UpdatePowerUI();
    }

    public void ToggleLight()
    {
        isLightOn = !isLightOn;
        lightComponent.enabled = isLightOn;
    }

    // Method to update the UI Slider and TextMeshPro text
    void UpdatePowerUI()
    {
        if (powerSlider != null)
        {
            powerSlider.value = power;
        }

        if (powerText != null)
        {
            powerText.text =  Mathf.RoundToInt(power) + "%";
        }
    }
}
