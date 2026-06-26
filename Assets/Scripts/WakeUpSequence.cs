using System.Collections;
using TMPro;
using UnityEngine;

public class WakeUpSequence : MonoBehaviour
{
    [Header("Settings")]
    public CanvasGroup blackScreen;
    public TextMeshProUGUI subtitleText;
    public AudioSource voiceAudio;
    public AudioSource heartAudio;
    public AudioClip introVoice;
    public MonoBehaviour playerMovement;
    public FlashlightController flashlight;

    void Start()
    {
        StartCoroutine(IntroRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && blackScreen.enabled == true)
        {
            StopCoroutine("IntroRoutine");
            if (blackScreen != null) blackScreen.gameObject.SetActive(false);
            if (subtitleText != null) subtitleText.gameObject.SetActive(false);
            if (voiceAudio != null) voiceAudio.Stop();
            if (heartAudio != null) heartAudio.Stop();
            if (playerMovement != null) playerMovement.enabled = true;
        }
    }

    IEnumerator IntroRoutine()
    {
        playerMovement.enabled = false;
        blackScreen.alpha = 1;

        if (flashlight != null)
            flashlight.isPaused = true;

        if (heartAudio != null)
        {
            heartAudio.volume = 0.7f;
            heartAudio.pitch = 0.8f;
            heartAudio.Play();
        }

        yield return new WaitForSeconds(1.5f);

        voiceAudio.PlayOneShot(introVoice);
        subtitleText.text = "Ugh... my head... where am I?";
        subtitleText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FedeAlpha(1f, 0.4f, 0.3f));
        yield return StartCoroutine(FedeAlpha(0.4f, 1f, 0.25f));

        yield return new WaitForSeconds(1.5f);

        subtitleText.text = "I don't... I don't remember how I got here. What happened? I... I need to find a way out. Now!";
        yield return StartCoroutine(FedeAlpha(1f, 0.2f, 0.4f));
        yield return StartCoroutine(FedeAlpha(0.2f, 1f, 0.2f));

        yield return new WaitForSeconds(5f);

        yield return StartCoroutine(FedeAlpha(1f, 0.2f, 0.4f));
        yield return StartCoroutine(FedeAlpha(0.2f, 1f, 0.2f));

        yield return new WaitForSeconds(1f);

        //yield return StartCoroutine(FedeAlpha(1f, 0f, 2f));

        float t = 0;
        float duration = 7f;
        while (t < 1)
        {
            t += Time.deltaTime / duration;

            blackScreen.alpha = 1 - t;

            if (heartAudio != null)
            {
                heartAudio.volume = Mathf.Lerp(0.8f, 0f, t);
            }

            yield return null;
        }

        //yield return new WaitForSeconds(3f);

        subtitleText.gameObject.SetActive(false);
        blackScreen.gameObject.SetActive(false);
        playerMovement.enabled = true;
        if (flashlight != null)
            flashlight.isPaused = false;

        //if(heartAudio != null)
        //    heartAudio.Stop();
    }

    IEnumerator FedeAlpha(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            blackScreen.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }
        blackScreen.alpha = endAlpha;
    }

}
