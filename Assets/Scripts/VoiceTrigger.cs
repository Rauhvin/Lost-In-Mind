using System.Collections;
using TMPro;
using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    [Header("Settings")]
    public TextMeshProUGUI subtitleText;
    public AudioSource playerAudioSource;
    public AudioClip voiceClip;
    public string textToDisplay = "Oh... oh my God... wh-what... what happened to him?!";
    public MonoBehaviour playerMovement;

    private bool hasPlayed = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && hasPlayed)
        {
            StopCoroutine("PlayVoiceSequence");
            if (subtitleText != null) subtitleText.gameObject.SetActive(false);
            if (playerAudioSource != null) playerAudioSource.Stop();
            if (playerMovement != null) playerMovement.enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            hasPlayed = true;
            playerMovement.enabled = false;
            StartCoroutine(PlayVoiceSequence());
        }
    }

    IEnumerator PlayVoiceSequence()
    {
        yield return new WaitForSeconds(0.5f);

        subtitleText.text = textToDisplay;
        subtitleText.gameObject.SetActive(true);

        if (playerAudioSource != null && voiceClip != null)
        {
            playerAudioSource.PlayOneShot(voiceClip);
            yield return new WaitForSeconds(voiceClip.length);
        }
        else
        {
            yield return new WaitForSeconds(2.5f);
        }

        yield return new WaitForSeconds(1f);
        subtitleText.gameObject.SetActive(false);
        playerMovement.enabled = true;
    }
}
