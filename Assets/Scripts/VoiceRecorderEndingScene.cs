using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoiceRecorderEndingScene : MonoBehaviour
{
    [Header("UI & Texts")]
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI subtitleText;

    [TextArea(3, 10)]
    public string[] dialogLines = new string[]
    {
        "Some text...."
    };

    [Header("DisplayText")]
    public float[] lineDurations = new float[] { 1f };

    [Header("Components")]
    public AudioSource dictaphoneAudioSource;
    public MonoBehaviour playerMovement;
    public GameObject doorSpotlight;
    public DoorControllerEndingScene doorScript;

    [Header("Background Music")]
    public AudioSource backgroundMusicSource;

    private bool isPlayerClose = false;
    private bool isPlayingSequence = false;

    private Coroutine musicFadeCoroutine;

    void Start()
    {
        if (promptText != null) promptText.gameObject.SetActive(false);
        if (subtitleText != null) subtitleText.gameObject.SetActive(false);
        if (doorScript != null) doorScript.enabled = false;


        if (dictaphoneAudioSource == null)
            dictaphoneAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlayerClose && !isPlayingSequence && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine("PlayDictaphoneSequence");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine("PlayDictaphoneSequence");
            if (promptText != null) promptText.gameObject.SetActive(false);
            if (subtitleText != null) subtitleText.gameObject.SetActive(false);
            if (dictaphoneAudioSource != null) dictaphoneAudioSource.Stop();
            if (doorSpotlight != null) doorSpotlight.SetActive(true);
            if (playerMovement != null) playerMovement.enabled = true;
            if (doorScript != null) doorScript.enabled = true;
            if (backgroundMusicSource != null) backgroundMusicSource.volume = 0.2f;
        }
    }

    IEnumerator PlayDictaphoneSequence()
    {
        isPlayingSequence = true;
        isPlayerClose = false;

        if (promptText != null) promptText.gameObject.SetActive(false);
        if (playerMovement != null) playerMovement.enabled = false;

        yield return new WaitForSeconds(0.3f);

        if (dictaphoneAudioSource != null && dictaphoneAudioSource.clip != null)
        {
            dictaphoneAudioSource.Play();
        }

        if (backgroundMusicSource != null)
        {
            ControlMusicFade(backgroundMusicSource, 4.0f, 0.05f);
        }

        if (subtitleText != null)
        {
            subtitleText.gameObject.SetActive(true);

            for (int i = 0; i < dialogLines.Length; i++)
            {
                subtitleText.text = dialogLines[i];
                
                float waitTime = (i < lineDurations.Length) ? lineDurations[i] : 4.0f;

                yield return new WaitForSeconds(waitTime);
            }
        }

        if (subtitleText != null) subtitleText.gameObject.SetActive(false);

        if (backgroundMusicSource != null)
        {
            ControlMusicFade(backgroundMusicSource, 3.0f, 0.2f);
        }

        if (doorSpotlight != null)
        {
            doorSpotlight.SetActive(true);
        }

        if (playerMovement != null) playerMovement.enabled = true;

        if (doorScript != null) doorScript.enabled = true;

    }

    private void ControlMusicFade(AudioSource audio, float fadeTime, float targetVolume)
    {
        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
        }
        musicFadeCoroutine = StartCoroutine(FadeMusicRoutine(audio, fadeTime, targetVolume));
    }

    IEnumerator FadeMusicRoutine(AudioSource audio, float fadeTime, float targetVolume)
    {
        float startVolume = audio.volume;
        float currentTime = 0f;

        if (targetVolume > 0 && !audio.isPlaying)
        {
            audio.Play();
        }

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            audio.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeTime);
            yield return null;
        }

        audio.volume = targetVolume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlayingSequence)
        {
            isPlayerClose = true;
            if (promptText != null)
            {
                promptText.text = "Press E to listen records";
                promptText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = false;
            if (promptText != null && !isPlayingSequence)
                promptText.gameObject.SetActive(false);
        }
    }
}
