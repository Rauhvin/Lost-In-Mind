using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Interaction UI")]
    //[SerializeField] private GameObject subtitlePanel;
    //public TextMeshProUGUI interactionText;
    public GameObject subtitlePanel;         
    public TextMeshProUGUI interactionText;

    [Header("Note System")]
    public GameObject notePanel;
    public TMPro.TextMeshProUGUI noteContentField;

    [Header("Photo Display")]
    public GameObject photoViewPanel;
    public UnityEngine.UI.Image photoImageHolder;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        //HideText();
    }

    public void ShowPhotoDisplay(Sprite photo)
    {
        photoViewPanel.SetActive(true);
        photoImageHolder.sprite = photo;
    }

    public void HidePhotoDisplay()
    {
        photoViewPanel.SetActive(false);
    }


    private object currentTextOwner = null; 

    public void ShowText(string message, object owner = null)
    {
        currentTextOwner = owner; 

        if (subtitlePanel != null)
        {
            subtitlePanel.SetActive(true);
        }

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(true);
            interactionText.text = message;
        }
    }

    public void HideText(object owner = null)
    {


        if (owner == null || currentTextOwner == owner)
        {
            if (subtitlePanel != null)
            {
                subtitlePanel.SetActive(false);
            }

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
            currentTextOwner = null;
        }
    }

    public void ShowNote(string content)
    {
        noteContentField.text = content;
        notePanel.SetActive(true);
    }

    public void HideNote()
    {
        notePanel.SetActive(false);
    }
}