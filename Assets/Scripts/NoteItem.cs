using UnityEngine;

public class NoteItem : MonoBehaviour
{
    [Header("Note data")]
    public string noteTitle = "Note title";
    [TextArea(10, 20)]
    public string fullText = "Content...";

    private bool isPlayerNearby = false;
    private bool isReading = false;
    private JournalController journalController;

    void Start()
    {
        journalController = FindObjectOfType<JournalController>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (isReading) CollectNote();
            else if (isPlayerNearby) OpenNote();
        }   
    }

    void OpenNote()
    {
        isReading = true;
        UIManager.Instance.ShowNote(fullText);

        if (journalController != null) journalController.SetPlayerControl(false);
    }

    void CollectNote()
    {
        PlayerInventory inv = FindAnyObjectByType<PlayerInventory>();
        if (inv != null)
        {
            NoteData newData = new NoteData { title = noteTitle, content = fullText };
            inv.AddNote(newData);
        }

        UIManager.Instance.HideNote();

        if (journalController != null) journalController.SetPlayerControl(true);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isReading)
        {
            isPlayerNearby = true;
            UIManager.Instance.ShowText("Press E to reading");
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
