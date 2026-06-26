using UnityEngine;

public class JournalController : MonoBehaviour
{
    public GameObject journalPanel;
    public MonoBehaviour playerMovement;
    public MonoBehaviour mouseLook;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (UIManager.Instance != null) UIManager.Instance.HideNote();

            ToggleJournal();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            bool isNoteOpen = UIManager.Instance.notePanel.activeSelf;
            bool isPhotoOpen = UIManager.Instance.photoViewPanel.activeSelf;

            if (UIManager.Instance != null && (isNoteOpen || isPhotoOpen))
            {
                UIManager.Instance.HideNote();
                UIManager.Instance.HidePhotoDisplay();

                if (!journalPanel.activeSelf)
                {
                    SetPlayerControl(true);
                }
            }
        }
    }

    public void ToggleJournal()
    {
        if (journalPanel == null) return;

        bool isActive = !journalPanel.activeSelf;
        journalPanel.SetActive(isActive);

        if (isActive)
        {
            journalPanel.GetComponent<JournalManager>().RefreshJournal();
            SetPlayerControl(false);
        }
        else
            SetPlayerControl(true);
    }

    public void SetPlayerControl(bool state)
    {
        if (playerMovement) playerMovement.enabled = state;
        if (mouseLook) mouseLook.enabled = state;

        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;

        if (UIManager.Instance != null) UIManager.Instance.HideText();
    }
}
