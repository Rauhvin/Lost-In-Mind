using UnityEngine;

public class activeTrigger : MonoBehaviour
{
    public GameObject underline;
    public AudioSource menuAudioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public void OnMouseEnter()
    {
        if (underline != null)
        {
            underline.SetActive(true);
        }

        if (menuAudioSource != null && hoverSound != null)
        {
            menuAudioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnMouseExit()
    {
        if (underline != null)
        {
            underline.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (menuAudioSource != null && clickSound != null)
        {
            menuAudioSource.PlayOneShot(clickSound);
        }
    }

        void OnDisable()
    {
        if (underline != null)
        {
            underline.SetActive(false);
        }
    }

}
