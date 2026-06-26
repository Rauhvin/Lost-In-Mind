using TMPro;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;       
    public KeyCode toggleKey = KeyCode.F;

    [Header("Battery Settings")]
    public float batteryLevel = 100f;
    public float consumptionRate = 1f;
    public TextMeshProUGUI batteryDisplay;
    public bool isPaused = false;

    private bool isOn = true;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (batteryLevel > 0)
            {
                isOn = !isOn;
                flashlight.enabled = isOn;
            }
        }

        if (isOn && batteryLevel > 0 && !isPaused)
        {
            batteryLevel -= consumptionRate * Time.deltaTime;

            if(batteryLevel < 10f)
            {
                if (Random.value > 0.9f)
                    flashlight.enabled = !flashlight.enabled;
            }
            else
            {
                flashlight.enabled = true;
            }
        }

        if (batteryLevel <= 0)
        {
            batteryLevel = 0;
            isOn = false;
            flashlight.enabled = false;
        }

        if (batteryLevel != null)
        {
            batteryDisplay.text = "Battery " + Mathf.RoundToInt(batteryLevel) + "%";
        }
    }

    public void AddBattery(float amount)
    {
        batteryLevel = Mathf.Clamp(batteryLevel + amount, 0, 100f);
    }
}