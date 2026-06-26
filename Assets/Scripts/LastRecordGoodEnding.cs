using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LastRecordGoodEnding : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI subtitleText;

    [TextArea(3, 10)]
    public string[] dialogLines = new string[]
    {
        "Some text...."
    };

    [Header("DisplayText")]
    public float[] lineDurations = new float[] { 1f };

    [Header("Components")]
    public AudioSource triggerAudioSource;
    public MonoBehaviour playerMovemenet;

    [Header("Background Music")]
    public AudioSource backgroundMusicSource;
    public AudioClip nextBackgroundMusic;

    private bool isPlayingSequence = false;

    private Coroutine musicFadeCoroutine;

    void Start()
    {
        if (subtitleText != null) subtitleText.gameObject.SetActive(false);


        if (triggerAudioSource == null)
            triggerAudioSource = GetComponent<AudioSource>();

        triggerAudioSource.Stop();
    }

    void Update()
    {
        if (isPlayingSequence && Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine("PlayDictaphoneSequence");

            if (triggerAudioSource != null) triggerAudioSource.Stop();
            if (subtitleText != null) subtitleText.gameObject.SetActive(false);

            if (backgroundMusicSource != null && nextBackgroundMusic != null)
            {
                if (musicFadeCoroutine != null) StopCoroutine(musicFadeCoroutine);
                backgroundMusicSource.Stop();
                backgroundMusicSource.clip = nextBackgroundMusic;
                backgroundMusicSource.volume = 0.5f; 
                backgroundMusicSource.Play();
            }

            if (playerMovemenet != null) playerMovemenet.enabled = true;

            gameObject.SetActive(false);
        }
    }


    IEnumerator PlayDictaphoneSequence()
    {
        isPlayingSequence = true;
        triggerAudioSource.Play();

        if (playerMovemenet != null) playerMovemenet.enabled = false;

        yield return new WaitForSeconds(0.3f);


        if (backgroundMusicSource != null)
        {
            ControlMusicFade(backgroundMusicSource, 3.0f, 0f);
        }

        if (triggerAudioSource != null && triggerAudioSource.clip != null)
        {
            triggerAudioSource.Play();
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

        yield return new WaitForSeconds(0.5f);

        if (backgroundMusicSource != null && nextBackgroundMusic != null)
        {
            backgroundMusicSource.Stop();
            backgroundMusicSource.clip = nextBackgroundMusic;

            backgroundMusicSource.volume = 0f;
            backgroundMusicSource.Play();
            ControlMusicFade(backgroundMusicSource, 1.0f, 0.5f);
            
        }
        if (playerMovemenet != null) playerMovemenet.enabled = true;

        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);

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
            StartCoroutine("PlayDictaphoneSequence");
        }
    }
}
