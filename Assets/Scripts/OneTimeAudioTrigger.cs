using UnityEngine;

public class OneTimeAudioTrigger : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip scareClip;          
    [Range(0f, 1f)]
    public float volume = 0.5f;          

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            hasPlayed = true; 

            if (scareClip != null)
            {
                AudioSource.PlayClipAtPoint(scareClip, other.transform.position, volume);
            }
        }
    }
}