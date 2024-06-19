using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class Flashlight : MonoBehaviour
{
    public GameObject lightObject; // Reference to the GameObject containing the 2D light
    public float power = 100f; // Initial power level, assuming full power is 100%
    private float powerDecayRate = 100f / 120f; // Decay rate to deplete power in 60 seconds

    public bool isLightOn = false;
    private Light2D lightComponent;
    public GameObject flashlightGameObject;

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
            // If the light is on, decrement power over time
            if (power > 0)
            {
                power -= powerDecayRate * Time.deltaTime;
            }

            // If power reaches zero, turn off the light
            if (power <= 0)
            {
                power = 0;
                lightComponent.enabled = false;
                flashlightGameObject.SetActive(false);

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
        if (power > 0) // Only allow toggling on if there is power
        {
            isLightOn = !isLightOn;
            lightComponent.enabled = isLightOn;
            flashlightGameObject.SetActive(isLightOn);
        }
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
            powerText.text = Mathf.RoundToInt(power) + "%";
        }
    }
}
