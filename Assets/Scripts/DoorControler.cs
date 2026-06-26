using UnityEngine;

public class SmartDoor : MonoBehaviour
{
    public float openAngle = 90f;
    public float smooth = 2f;
    public string requiredItem = "Key_Blue";

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private bool isPlayerNearby = false;
    private PlayerInventory playerInv;
    private Quaternion defaultRotation;

    void Start() => defaultRotation = transform.localRotation;

    void Update()
    {
        if (isPlayerNearby)
        {
            UpdateUI();

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (string.IsNullOrEmpty(requiredItem) || playerInv.HasItem(requiredItem))
                {
                    isOpen = !isOpen;

                    if (isOpen)
                        PlaySound(openSound);
                    else
                        PlaySound(closeSound);
                }
            }
        }

        Quaternion openRotation = defaultRotation * Quaternion.Euler(0, openAngle, 0);
        Quaternion target = isOpen ? openRotation : defaultRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smooth);
    }

    void UpdateUI()
    {
        if (string.IsNullOrEmpty(requiredItem) || playerInv.HasItem(requiredItem))
        {
            UIManager.Instance.ShowText(isOpen ? "Press E to close" : "Press E to open", this);
        }
        else
        {
            UIManager.Instance.ShowText("You need a key", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            playerInv = other.GetComponent<PlayerInventory>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            UIManager.Instance.HideText(this); 
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clip);
        }
    }
}