using System.Collections;
using UnityEngine;

public class FireRepel : MonoBehaviour
{
    [Header("Visuals and Sounds")]
    public GameObject redScreenImage;
    public AudioSource heartAudio;

    [Header("Player Controller")]
    public MonoBehaviour playerMovement;
    public Transform playerCamera;

    [Header("Bounce Settings")]
    public float bounceBackForce = 3f;
    public float fallDuration = 2.0f;

    private bool isProcessingDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isProcessingDamage)
        {
            StartCoroutine(FireSequence(other.gameObject));
        }
    }

    IEnumerator FireSequence(GameObject player)
    {
        isProcessingDamage = true;

        if (playerMovement  != null) playerMovement.enabled = false;

        if (redScreenImage != null) redScreenImage.SetActive(true);

        if (heartAudio != null)
        {
            heartAudio.volume = 0.9f;
            heartAudio.pitch = 0.8f;
            if (!heartAudio.isPlaying) heartAudio.Play();
        }

        Vector3 pushDirection = (player.transform.position - transform.position).normalized;
        pushDirection.y = 0;
        pushDirection.x = 0;

        float elapsed = 0f;
        Vector3 originalCamPos = playerCamera.localPosition;
        Vector3 fallenCamPos = new Vector3(originalCamPos.x, 0.2f, originalCamPos.z);

        while (elapsed < fallDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fallDuration;

            player.transform.position += pushDirection * bounceBackForce * Time.deltaTime;

            if (t < 0.2f)
            {
                playerCamera.localPosition = Vector3.Lerp(originalCamPos, fallenCamPos, t / 0.2f);
            }
            else if (t > 0.6f)
            {
                float recoveryT = (t - 0.6f) / 0.4f;

                playerCamera.localPosition = Vector3.Lerp(fallenCamPos, originalCamPos, recoveryT);

                if (heartAudio != null)
                {
                    heartAudio.volume = Mathf.Lerp(0.9f, 0f, recoveryT);
                    heartAudio.pitch = Mathf.Lerp(1.5f, 0.8f, recoveryT);
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        playerCamera.localPosition = originalCamPos;
        if (redScreenImage != null) redScreenImage.SetActive(false);
        if (playerMovement != null) playerMovement.enabled = true;
        if (heartAudio != null) heartAudio.Stop();

        isProcessingDamage = false;
    }
}
