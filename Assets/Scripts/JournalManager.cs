using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class JournalManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform container;
    public PlayerInventory inventory;

    private List<GameObject> spawnedButtons = new List<GameObject>();

    public void RefreshJournal()
    {
        if (inventory == null) inventory = FindAnyObjectByType<PlayerInventory>();

        foreach (var btn in spawnedButtons) Destroy(btn);
        spawnedButtons.Clear();

        foreach (NoteData note in inventory.collectedNotes)
        {
            GameObject newBtn = Instantiate(buttonPrefab, container);
            newBtn.GetComponentInChildren<TextMeshProUGUI>().text = note.title;

            newBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                JournalController controller = FindObjectOfType<JournalController>();
                if (note.noteImage != null)
                {
                    if (controller != null) controller.ToggleJournal();

                    UIManager.Instance.ShowPhotoDisplay(note.noteImage);
                }
                else
                {
                    UIManager.Instance.ShowNote(note.content);
                }
            });

            spawnedButtons.Add(newBtn);
        }
    }
}
