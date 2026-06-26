using UnityEngine;

public class BatteryItem : MonoBehaviour
{
    [Header("Settings")]
    public float energyAmount = 25f;
    public string itemName = "Battery";
    public KeyCode interactKey = KeyCode.E;

    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(interactKey))
        {
            CollectBattery();
        }
    }

    void CollectBattery()
    {
        FlashlightController flashlight = FindObjectOfType<FlashlightController>();

        if (flashlight != null)
        {
            flashlight.AddBattery(energyAmount);
            UIManager.Instance.HideText();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            UIManager.Instance.ShowText("Press " + interactKey + " to raise " + itemName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            UIManager.Instance.HideText();
        }
    }
}
