using UnityEngine;

public class PhotoItem : MonoBehaviour
{
    public FireManager fireManager;

    [Header("Photo Settings")]
    public string photoTitle = "Old Photo";
    public Sprite photoSprite;

    [Header("Dialog System")]
    public VoiceTrigger voiceTrigger;
    public bool playVoiceOnlyOnce = true;
    private static bool globalVoicePlayed = false;

    private bool isPlayerNearby = false;
    private bool isShowing = false;
    private JournalController journalController;

    void Start()
    {
        journalController = FindObjectOfType<JournalController>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (isShowing) CollectPhoto();
            else if (isPlayerNearby) OpenPhoto();
        }
    }

    void OpenPhoto()
    {
        isShowing = true;
        UIManager.Instance.ShowPhotoDisplay(photoSprite);

        if (voiceTrigger != null )
        {
            if (!playVoiceOnlyOnce || !globalVoicePlayed)
            {
                if (journalController != null) journalController.SetPlayerControl(false);

                voiceTrigger.StartCoroutine("PlayVoiceSequence");

                if (playVoiceOnlyOnce) globalVoicePlayed=true;
            }
        }

        if (journalController != null) journalController.SetPlayerControl(false);

    }

    void CollectPhoto()
    {
        PlayerInventory inv = FindAnyObjectByType<PlayerInventory>();
        if (inv != null)
        {
            NoteData newData = new NoteData { title = photoTitle, content = "", noteImage = photoSprite };
            inv.AddNote(newData);
        }

        UIManager.Instance.HidePhotoDisplay();

        if (journalController != null) journalController.SetPlayerControl(true);

        fireManager.StopFire();

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            UIManager.Instance.ShowText("Press E to view the photo");
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