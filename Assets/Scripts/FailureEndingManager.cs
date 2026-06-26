using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailureEndingManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI terminalText;
    public GameObject finalQuoteObject;

    [Header("Audio Sources")]
    public AudioSource sourceEKG;
    public AudioSource sourceDefib;

    [Header("Audio Clips")]
    public AudioClip ekgBeepClip;
    public AudioClip defibAndFlatlineClip;

    [Header("Synchronization file 2")]
    public float chargingDuration = 2.5f;

    [Header("Time Loop")]
    public string firstRoomSceneName = "Scene_FirstRoom";

    void Start()
    {
        if (terminalText != null) terminalText.text = "";
        if (finalQuoteObject != null) finalQuoteObject.SetActive(false);
        if (sourceEKG != null) sourceEKG.Stop();
        if (sourceDefib != null) sourceDefib.Stop();

        StartCoroutine(PlayFailureSequenceRoutine());
    }

    IEnumerator PlayFailureSequenceRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        if (sourceEKG != null && ekgBeepClip != null)
        {
            sourceEKG.clip = ekgBeepClip;
            sourceEKG.Play();
        }

        if (terminalText != null)
        {
            terminalText.text = ">>> CRITICAL SYSTEM FAILURE: LIFE SUPPORT INTERRUPTED <<<\n";
            yield return new WaitForSeconds(1.5f);
            terminalText.text += "\n[00:00:01] WARNING: ACUTE BRADYCARDIA DETECTED";
            yield return new WaitForSeconds(1.5f);
            terminalText.text += "\n[00:00:05] PATIENT RESPIRATION: STOPPED";
            yield return new WaitForSeconds(1.5f);
            terminalText.text += "\n[00:00:08] HEART RATE: 42 BPM... 18 BPM... 0 BPM";
        }

        yield return new WaitForSeconds(1.5f);

        if (terminalText != null)
        {
            terminalText.text += "\n[00:00:11] EMERGENCY: ASYSTOLE DETECTED (FLATLINE)";
        }

        yield return new WaitForSeconds(1.0f);

        if (sourceEKG != null)
        {
            sourceEKG.Stop();
        }

        if (sourceDefib != null && defibAndFlatlineClip != null)
        {
            sourceDefib.clip = defibAndFlatlineClip;
            sourceDefib.Play();
        }

        if (terminalText != null)
        {
            terminalText.text += "\n[00:00:13] INITIATING CARDIAC DEFIBRILLATION...";
        }

        yield return new WaitForSeconds(chargingDuration);

        if (terminalText != null)
        {
            terminalText.text += "\n[00:00:16] SHOCK DELIVERED: 360 JOULES --- NO RESPONSE";
            yield return new WaitForSeconds(1.5f);
            terminalText.text += "\n[00:00:18] BRAIN ACTIVITY: CRITICAL DROP";
            yield return new WaitForSeconds(1.5f);
            terminalText.text += "\n[00:00:21] NEURAL LINK: SYNC LOST";
            yield return new WaitForSeconds(1.5f);
            terminalText.text += "\n\n---------------------------------------------------------";
            terminalText.text += "\nSTATUS: PATIENT TERMINATED // TIME OF DEATH: 00:00:00";
            terminalText.text += "\n---------------------------------------------------------";
        }

        yield return new WaitForSeconds(3.0f);

        if (terminalText != null)
        {
            terminalText.text = "";
        }

        if (finalQuoteObject != null)
        {
            finalQuoteObject.SetActive(true);
        }

        yield return new WaitForSeconds(7.0f);

        if (finalQuoteObject != null) finalQuoteObject.SetActive(false);

        if (terminalText != null)
        {
            terminalText.text = ">>> AUTOMATIC SYSTEM REBOOT INITIATED IN 3... 2... 1...";
        }

        yield return new WaitForSeconds(3.0f);

        if (sourceDefib != null)
        {
            sourceDefib.Stop();
        }

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(firstRoomSceneName);
    }
}