using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    private static List<string> globalItems = new List<string>();
    private static List<NoteData> globalNotes = new List<NoteData>();

    public List<string> collectedItems = new List<string>();
    public List<NoteData> collectedNotes = new List<NoteData>();

    [Header("UI Settings")]
    public TextMeshProUGUI inventoryDisplay;
    public GameObject inventoryPanel;

    void Awake()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "FirstScene")
        {
            ResetPersistentInventory();
        }

        collectedItems = new List<string>(globalItems);
        collectedNotes = new List<NoteData>(globalNotes);
    }

    void Update()
    {
        if (inventoryDisplay == null || inventoryPanel == null)
            return;

        if (Input.GetKey(KeyCode.Tab))
        {
            inventoryDisplay.gameObject.SetActive(true);
            inventoryPanel.SetActive(true);

            UpdateInventoryUI();
        }
        else
        {
            inventoryDisplay.gameObject.SetActive(false);
            inventoryPanel.SetActive(false);
        }
    }

    public void AddNote(NoteData newNote)
    {
        collectedNotes.Add(newNote);
        globalNotes.Add(newNote); 
        Debug.Log("Collected note: " + newNote.title);
    }

    public void AddItem(string name)
    {
        collectedItems.Add(name);
        globalItems.Add(name); 
        Debug.Log("Collected: " + name);
    }

    public bool HasItem(string name)
    {
        return collectedItems.Contains(name);
    }

    public static void ResetPersistentInventory()
    {
        globalItems.Clear();
        globalNotes.Clear();
    }

    void UpdateInventoryUI()
    {
        if (inventoryDisplay == null)
            return;

        if (collectedItems.Count == 0)
            inventoryDisplay.text = "Inventory is empty...";
        else
        {
            inventoryDisplay.text = "<b>COLLECTED ITEMS:</b>\n";
            foreach (string item in collectedItems)
                inventoryDisplay.text += "- " + item + "\n";
        }
    }
}