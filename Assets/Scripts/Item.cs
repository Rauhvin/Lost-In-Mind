using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public string itemName = "Key_Blue";
    public KeyCode interactKey = KeyCode.E;
    public bool canBePickedUp = true;

    private bool isPlayerNearby = false;
    private PlayerInventory playerInv;

    void Update()
    {
        if (canBePickedUp && isPlayerNearby && Input.GetKeyDown(interactKey))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        if (playerInv != null)
        {
            playerInv.AddItem(itemName);

            UIManager.Instance.HideText();

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canBePickedUp) return;

        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            playerInv = other.GetComponent<PlayerInventory>();

            UIManager.Instance.ShowText("Press E to raise key");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            playerInv = null;

            UIManager.Instance.HideText();
        }
    }
}