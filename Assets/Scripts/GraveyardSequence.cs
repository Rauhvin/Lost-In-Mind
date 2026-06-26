using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GraveyardEndingManager : MonoBehaviour
{
    [Header("UI & Texts (Scena i Finał)")]
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI subtitleText;
    public Image blackFadeImage;            

    [TextArea(3, 10)]
    public string[] dialogLines = new string[]
    {
        "To... to nie był żaden pożar śledztwa.",
        "Ona odeszła tamtego dnia.",
        "A ja utknąłem w tym korytarzu na zawsze."
    };

    [Header("Display Times")]
    public float[] lineDurations = new float[] { 4.0f, 4.0f, 5.0f };
    public float transitionDuration = 4f;
    public float fadeToBlackDuration = 2.5f;

    [Header("Plansze Końcowe z GIMPa")]
    [Tooltip("Przeciągnij tutaj pierwszy obrazek UI z napisem fabularnym")]
    public GameObject firstEndingImage;
    [Tooltip("Czas wyświetlania pierwszej planszy (np. 6.5 sekundy)")]
    public float firstImageDuration = 6.5f;
    [Tooltip("Przeciągnij tutaj drugi obrazek UI (Tytuł, autorzy, podziękowania)")]
    public GameObject creditsEndingImage;

    [Header("Znikająca Kobieta & Nagrobek")]
    public Renderer[] womanRenderers;
    public float womanFadeDuration = 3.0f;
    public GameObject tombstoneObject;

    [Header("Kamery (Wzór z Menu)")]
    public GameObject playerCameraObject;
    public Transform cutsceneCameraTransform;

    [Header("Components & Audio")]
    public AudioSource voiceAudioSource;
    public MonoBehaviour playerMovement;

    [Header("Background Music")]
    public AudioSource backgroundMusicSource;
    public AudioClip finalMournfulMusic;

    [Header("Next scene")]
    public string menuSceneName = "menuScene";

    private bool isPlayerClose = false;
    private bool isPlayingSequence = false;
    private Coroutine musicFadeCoroutine;

    void Start()
    {
        if (voiceAudioSource != null) voiceAudioSource.Stop();
        if (promptText != null) promptText.gameObject.SetActive(false);
        if (subtitleText != null) subtitleText.gameObject.SetActive(false);

        if (firstEndingImage != null) firstEndingImage.SetActive(false);
        if (creditsEndingImage != null) creditsEndingImage.SetActive(false);

        if (blackFadeImage != null)
        {
            blackFadeImage.gameObject.SetActive(true);
            Color c = blackFadeImage.color;
            c.a = 0f;
            blackFadeImage.color = c;
            blackFadeImage.gameObject.SetActive(false);
        }

        if (voiceAudioSource == null)
            voiceAudioSource = GetComponent<AudioSource>();

        if (tombstoneObject != null)
            tombstoneObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerClose && !isPlayingSequence)
        {
            StartCoroutine("PlayEndingSequence");
        }

        if (isPlayingSequence && Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine("PlayEndingSequence");
            if (voiceAudioSource != null) voiceAudioSource.Stop();
            if (backgroundMusicSource != null) backgroundMusicSource.Stop();
            if (playerMovement != null) playerMovement.enabled = true;

            SceneManager.LoadScene(menuSceneName);
        }
    }

    IEnumerator PlayEndingSequence()
    {
        isPlayingSequence = true;
        isPlayerClose = false;

        if (promptText != null) promptText.gameObject.SetActive(false);
        if (playerMovement != null) playerMovement.enabled = false;

        yield return new WaitForSeconds(0.3f);

        if (backgroundMusicSource != null)
        {
            ControlMusicFade(backgroundMusicSource, 2.0f, 0.02f);
        }

        float fadeElapsed = 0f;
        while (fadeElapsed < womanFadeDuration)
        {
            fadeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeElapsed / womanFadeDuration);

            foreach (Renderer rend in womanRenderers)
            {
                if (rend != null && rend.material.HasProperty("_Color"))
                {
                    Color c = rend.material.color;
                    c.a = alpha;
                    rend.material.color = c;
                }
            }
            yield return null;
        }

        if (womanRenderers.Length > 0 && womanRenderers[0] != null)
        {
            womanRenderers[0].transform.root.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        if (tombstoneObject != null)
        {
            tombstoneObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        if (cutsceneCameraTransform != null)
        {
            Vector3 targetPosition = cutsceneCameraTransform.position;
            Quaternion targetRotation = cutsceneCameraTransform.rotation;

            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cutsceneCameraTransform.position = mainCam.transform.position;
                cutsceneCameraTransform.rotation = mainCam.transform.rotation;
            }

            if (playerCameraObject != null) playerCameraObject.SetActive(false);
            cutsceneCameraTransform.gameObject.SetActive(true);

            float transitionElapsed = 0f;
            Vector3 startPosition = cutsceneCameraTransform.position;
            Quaternion startRotation = cutsceneCameraTransform.rotation;

            while (transitionElapsed < transitionDuration)
            {
                transitionElapsed += Time.deltaTime;
                float t = transitionElapsed / transitionDuration;

                t = Mathf.SmoothStep(0f, 1f, t);

                cutsceneCameraTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
                cutsceneCameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            cutsceneCameraTransform.position = targetPosition;
            cutsceneCameraTransform.rotation = targetRotation;
        }

        if (voiceAudioSource != null && voiceAudioSource.clip != null)
        {
            voiceAudioSource.Play();
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
            subtitleText.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(2.5f);

        if (blackFadeImage != null) blackFadeImage.gameObject.SetActive(true);

        float blackElapsed = 0f;
        while (blackElapsed < fadeToBlackDuration)
        {
            blackElapsed += Time.deltaTime;
            if (blackFadeImage != null)
            {
                Color c = blackFadeImage.color;
                c.a = Mathf.Lerp(0f, 1f, blackElapsed / fadeToBlackDuration);
                blackFadeImage.color = c;
            }
            yield return null;
        }

        if (backgroundMusicSource != null && finalMournfulMusic != null)
        {
            backgroundMusicSource.Stop();
            backgroundMusicSource.clip = finalMournfulMusic;
            backgroundMusicSource.volume = 0f;
            backgroundMusicSource.Play();
            ControlMusicFade(backgroundMusicSource, 3.0f, 0.7f); 
        }

        yield return new WaitForSeconds(1.0f);

        if (firstEndingImage != null)
        {
            firstEndingImage.SetActive(true);
        }

        yield return new WaitForSeconds(firstImageDuration);

        if (firstEndingImage != null) firstEndingImage.SetActive(false);

        if (creditsEndingImage != null)
        {
            creditsEndingImage.SetActive(true);
        }

        yield return new WaitForSeconds(backgroundMusicSource.clip.length + 2f);
        if (playerMovement != null) playerMovement.enabled = true;
        SceneManager.LoadScene(menuSceneName);

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
                promptText.text = "Press E to focus";
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