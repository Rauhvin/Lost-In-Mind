using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float timeRemaining = 300f;
    private bool timerIsRunning = false;
    private bool isLate = false;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Visual Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;
    [SerializeField] private float warningThreshold = 60f;

    [Header("Music")]
    public AudioSource backgroundMusicSource;
    public AudioClip phase1Clip;
    public AudioClip phase2Clip;
    public AudioClip phase3Clip;
    public AudioClip phase4Clip;
    public float fadeDuration = 1.5f;

    private float targetVolume = 1f;
    private AudioClip targetClip;
    private Coroutine musicTransitionCoroutine;

    void Start()
    {

        if (timerText != null)
        {
            timerText.color = normalColor;
        }

        if (backgroundMusicSource != null)
        {
            targetVolume = backgroundMusicSource.volume;
        }


        timerIsRunning = true;

        if (backgroundMusicSource != null && phase1Clip != null)
        {
            targetClip = phase1Clip;
            backgroundMusicSource.clip = phase1Clip;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {

                timeRemaining -= Time.deltaTime;


                DisplayTime(timeRemaining);
                UpdateMusicAndVisuals();

                if (timeRemaining <= 0 && !isLate)
                {
                    isLate = true;
                    OnTimerEnd();
                }
            }
           
        }

    }

    void UpdateMusicAndVisuals()
    {
        if (backgroundMusicSource == null) return;

        if (timeRemaining > 180f) 
        {
            ChangeMusicClipSmoothly(phase1Clip);
            if (timerText != null) timerText.color = normalColor;
        }
        else if (timeRemaining > 60f)
        {
            ChangeMusicClipSmoothly(phase2Clip);
            if (timerText != null) timerText.color = normalColor;
        }
        else if (timeRemaining > 0f)
        {
            ChangeMusicClipSmoothly(phase3Clip);
            if (timerText != null) timerText.color = warningColor;
        }
        else
        {
            ChangeMusicClipSmoothly(phase4Clip);
            if (timerText != null) timerText.color = warningColor;
        }
    }

    void ChangeMusicClipSmoothly(AudioClip newClip)
    {
        if (newClip == null) return;

        if (targetClip != newClip)
        {
            targetClip = newClip;

            if (musicTransitionCoroutine != null)
                StopCoroutine(musicTransitionCoroutine);

            musicTransitionCoroutine = StartCoroutine(FadeTrackSequence(newClip));
        }
    }

    IEnumerator FadeTrackSequence(AudioClip newClip)
    {
        float timer = 0f;
        float startVolume = backgroundMusicSource.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            backgroundMusicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }
        backgroundMusicSource.volume = 0f;

        backgroundMusicSource.clip = newClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();

        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            backgroundMusicSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeDuration);
            yield return null;
        }
        backgroundMusicSource.volume = targetVolume;

        musicTransitionCoroutine = null;
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0) timeToDisplay = 0;


        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);


        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }




    void OnTimerEnd()
    {
        //nothing
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public bool IsTimerRunning()
    {
        return timerIsRunning;
    }

    public void StopTimer() => timerIsRunning = false;
    public void StartTimer() => timerIsRunning = true;
}