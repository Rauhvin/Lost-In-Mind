using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DevConsoleManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject consolePanel;
    [SerializeField] private TMP_InputField inputField;

    [Header("Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.BackQuote;

    private bool isConsoleActive = false;

    void Start()
    {
        if (consolePanel != null) consolePanel.SetActive(false);

        if (inputField != null)
        {
            inputField.onSubmit.AddListener(ProcessCommand);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleConsole();
        }
    }

    void ToggleConsole()
    {
        isConsoleActive = !isConsoleActive;
        consolePanel.SetActive(isConsoleActive);

        if (isConsoleActive)
        {
            inputField.text = "";
            inputField.ActivateInputField();

            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void ProcessCommand(string command)
    {
        if (!isConsoleActive) return;

        string cleanCommand = command.Trim().ToLower();

        if (string.IsNullOrEmpty(cleanCommand)) return;

        switch (cleanCommand)
        {
            case "scene1":
            case "menu":
                SceneManager.LoadScene("Menu");
                break;

            case "scene2":
            case "first":
                SceneManager.LoadScene("FirstScene");
                break;

            case "scene3":
            case "main":
                Debug.Log("dzila");
                SceneManager.LoadScene("MainEscapeScene");
                break;

            case "good":
            case "scene4":
                SceneManager.LoadScene("Ending - Truth");
                break;

            case "bad":
            case "scene5":
                SceneManager.LoadScene("End1-bad");
                break;

            case "mute":
                AudioListener.volume = 0f;
                break;

            case "unmute":
                AudioListener.volume = 1f;
                break;

            default:
                Debug.LogWarning("Nieznana komenda: " + cleanCommand);
                inputField.text = "";
                inputField.ActivateInputField();
                return;
        }

        ToggleConsole();
    }

    void SetPlayerMovement(bool state)
    {
        var journal = FindObjectOfType<JournalController>();
        if (journal != null)
        {
            journal.SetPlayerControl(state);
        }
    }
}