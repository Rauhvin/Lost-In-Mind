using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneIntroManager : MonoBehaviour
{
    [Header("UI Components")]
    public Image blackFadeImage;
    public TextMeshProUGUI introText;

    [Header("Timing Settings")]
    public float fadeOutDuration = 2.0f;   
    public float textDisplayDuration = 6.0f; 
    public float textFadeDuration = 1.5f;  

    [Header("Player Control")]
    public MonoBehaviour playerMovement;
    public MonoBehaviour mouseLook;

    void Start()
    {
        PlayerInventory.ResetPersistentInventory();
        if (blackFadeImage != null)
        {
            blackFadeImage.gameObject.SetActive(true);
            Color c = blackFadeImage.color;
            c.a = 1f;
            blackFadeImage.color = c;
        }

        if (introText != null)
        {
            introText.gameObject.SetActive(false);
        }

        if (playerMovement != null) playerMovement.enabled = false;
        if (mouseLook != null) mouseLook.enabled = false;

        StartCoroutine("PlayIntroSequence");
    }

    IEnumerator PlayIntroSequence()
    {
        yield return new WaitForSeconds(0.4f);

        if (blackFadeImage != null)
        {
            float elapsed = 0f;
            Color c = blackFadeImage.color;

            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.deltaTime;
                c.a = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
                blackFadeImage.color = c;
                yield return null;
            }

            blackFadeImage.gameObject.SetActive(false);
        }

        if (playerMovement != null) playerMovement.enabled = true;
        if (mouseLook != null) mouseLook.enabled = true;

        if (introText != null)
        {
            introText.gameObject.SetActive(true);

            Color tc = introText.color;
            tc.a = 1f;
            introText.color = tc;

            yield return new WaitForSeconds(textDisplayDuration);

            float textElapsed = 0f;
            while (textElapsed < textFadeDuration)
            {
                textElapsed += Time.deltaTime;
                tc.a = Mathf.Lerp(1f, 0f, textElapsed / textFadeDuration);
                introText.color = tc;
                yield return null;
            }

            introText.gameObject.SetActive(false);
        }
    }
}