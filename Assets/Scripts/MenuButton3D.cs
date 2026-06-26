using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButton3D : MonoBehaviour
{
    [Header("Button type (Pick only one!)")]
    public bool isStartButton = false;
    public bool isTogglePanelButton = false;
    public bool isExitButton = false;

    [Header("Next scene")]
    public string sceneToLoad = "";

    [Tooltip("Main camera")]
    public Transform cameraTransform;

    [Tooltip("Duration time")]
    public float transitionDuration = 4f;

    [Header("Menu music")]
    public AudioSource backgroundMusic;

    [Header("Instruction settings")]
    public GameObject panelToToggle;

    private bool isTransitioning = false;

    private void Start()
    {
        PlayerInventory.ResetPersistentInventory();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnMouseDown()
    {
        if (isTransitioning) return;

        if (isStartButton && !string.IsNullOrEmpty(sceneToLoad) && cameraTransform != null)
        {
            StartCoroutine(PlayMenuTransition());
        }
        else if (isTogglePanelButton && panelToToggle != null)
        {
            panelToToggle.SetActive(!panelToToggle.activeSelf);
        }
        else if (isExitButton)
        {
            Application.Quit();
        }
    }

    IEnumerator PlayMenuTransition()
    {
        isTransitioning = true;
        float startVolume = 0f;
        if (backgroundMusic != null)
        {
            startVolume = backgroundMusic.volume;
        }

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

            float transitionElapsed = 0f;
            Vector3 startPosition = cameraTransform.position;
            Quaternion startRotation = cameraTransform.rotation;

            while (transitionElapsed < transitionDuration)
            {
                transitionElapsed += Time.deltaTime;
                float t = transitionElapsed / transitionDuration;

                if (backgroundMusic != null)
                {
                    backgroundMusic.volume = Mathf.Lerp(startVolume, 0f, t);
                }

                t = Mathf.SmoothStep(0f, 1f, t);

                cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
                cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            cameraTransform.position = targetPosition;
            cameraTransform.rotation = targetRotation;

            if (backgroundMusic != null) backgroundMusic.volume = 0f;
        }

        yield return new WaitForSeconds(1f); 

        
        SceneManager.LoadScene(sceneToLoad);
    }
}