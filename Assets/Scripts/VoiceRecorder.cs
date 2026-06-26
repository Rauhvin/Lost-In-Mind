using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoiceRecorder : MonoBehaviour
{
    [Header("UI & Texts")]
    public TextMeshProUGUI promptText;      
    public TextMeshProUGUI subtitleText;
    [TextArea(3, 10)]
    public string[] dialogLines = new string[]
    {
        "Dr. Evans recording. Progress report on Patient 512.",
        "We are initiating the final phase of Project Anamnesis.",
        "The patient has been in a comatose state for three months... following the severe fire at his residence.",
        "His subconscious has constructed a psychological defense mechanism... a loop.",
        "If we cannot force his consciousness to face the trauma within the next five minutes of deep synchronization...",
        "...the brain death will become permanent.",
        "Initiate the simulation now."
    };

    [Header("Display time")]
    public float[] lineDurations = new float[] { 7.0f, 6.0f, 7.0f, 7.0f, 8.0f, 3.5f, 3.0f };

    [Header("Final sequence")]
    public string nextSceneName = "MainEscapeScene";
    [TextArea(2, 5)]
    public string playerFinalWords = "It's... it's me. I am Patient 412. It was a fire... It wasn't an investigation." +
        "I was there. Oh god, I left her in the room... I couldn't open the door. No... no, I am not losing her again. Not like this!";
    public Image fadeImage;
    public AudioLowPassFilter cameraFilter;
    public Transform cameraTransform;
    public AudioSource playerAudioSource;
    public AudioClip playerVoiceClip;

    [Header("Components")]
    public AudioSource dictaphoneAudioSource; 
    public MonoBehaviour playerMovement;

    [Header("Background Music")]
    public AudioSource backgroundMusicSource;

    private bool isPlayerClose = false;
    private bool isPlayingSequence = false;
    private Vector3 originalCameraPos;

    void Start()
    {
        if (promptText != null) promptText.gameObject.SetActive(false);
        if (subtitleText != null) subtitleText.gameObject.SetActive(false);
        if (fadeImage != null) fadeImage.gameObject.SetActive(false);
        if (cameraFilter != null) cameraFilter.gameObject.SetActive(false);

        if (dictaphoneAudioSource == null)
            dictaphoneAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlayerClose && !isPlayingSequence && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine("PlayDictaphoneSequence");
        }

        if (isPlayingSequence && Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine("PlayDictaphoneSequence");
            if (dictaphoneAudioSource != null) dictaphoneAudioSource.Stop();
            if (playerAudioSource != null) playerAudioSource.Stop();

            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
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
            StartCoroutine(FadeOutMusic(backgroundMusicSource, 2.0f));
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

        yield return new WaitForSeconds(1.5f);

        if (subtitleText != null)
        {
            subtitleText.text = playerFinalWords;
        }

        if (playerAudioSource != null && playerVoiceClip != null)
        {
            playerAudioSource.PlayOneShot(playerVoiceClip);
        }

        yield return new WaitForSeconds(2.0f);

        if (cameraTransform != null)
        {
            Vector3 targetPosition = cameraTransform.position;
            Quaternion targetRotation = cameraTransform.rotation;

            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cameraTransform.position = mainCam.transform.position;
                cameraTransform.rotation = mainCam.transform.rotation;
            }

            cameraTransform.gameObject.SetActive(true);

            float transitionDuration = 1.5f;
            float transitionElapsed = 0f;
            Vector3 startPosition = cameraTransform.position;
            Quaternion startRotation = cameraTransform.rotation;

            while (transitionElapsed < transitionDuration)
            {
                transitionElapsed += Time.deltaTime;
                float t = transitionElapsed / transitionDuration;

                t = Mathf.SmoothStep(0f, 1f, t);

                cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
                cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            cameraTransform.position = targetPosition;
            cameraTransform.rotation = targetRotation;
        }

        if (cameraFilter != null) cameraFilter.enabled = true;
        if (fadeImage != null) fadeImage.gameObject.SetActive(true);

        float duration = 26.0f;
        float elapsed = 0f;
        float shakeMagnitude = 0.05f;

        float shakeUpdateRate = 0.05f;
        float nextShakeTime = 0f;

        if (cameraTransform != null) originalCameraPos = cameraTransform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = Mathf.Lerp(0f, 1f, progress);
                fadeImage.color = c;
            }

            if (cameraTransform != null)
            {
                if (elapsed >= nextShakeTime)
                {
                    float x = Random.Range(-1f, 1f) * shakeMagnitude;
                    float y = Random.Range(-1f, 1f) * shakeMagnitude;
                    cameraTransform.localPosition = new Vector3(originalCameraPos.x + x, originalCameraPos.y + y, originalCameraPos.z);
                    nextShakeTime = elapsed + shakeUpdateRate;
                }
            }

            yield return null;
        }

        if (cameraTransform != null) cameraTransform.localPosition = originalCameraPos;
        if (subtitleText != null) subtitleText.gameObject.SetActive(false);

        yield return new WaitForSeconds(3.0f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);

    }

    IEnumerator FadeOutMusic(AudioSource audio, float fadeTime)
    {
        float startVolume = audio.volume;

        while (audio.volume > 0)
        {
            audio.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audio.Stop();
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