using UnityEngine;
using System.Collections;

public class HorrorLightFlicker : MonoBehaviour
{
    private Light lightSource;

    [Header("Intesity settings")]
    public float minIntensity = 0.2f;
    public float maxIntensity = 1.5f;

    [Header("Time settings")]
    public float minDelay = 0.05f;
    public float maxDelay = 0.2f;

    void Start()
    {
        lightSource = GetComponent<Light>();
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            lightSource.intensity = Random.Range(minIntensity, maxIntensity);

            float randomTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(randomTime);
        }
    }
}