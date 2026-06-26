using UnityEngine;

public class FlashlightReveal : MonoBehaviour
{
    [Header("Settings")]
    public Transform flashlightTransform;
    public float maxDistance = 5f;
    public float viewAngle = 15f;

    private Renderer textRenderer;
    private Material textMaterial;


    void Start()
    {
        textRenderer = GetComponent<Renderer>();
        if (textRenderer != null)
        {
            textMaterial = textRenderer.material;
            SetTextAlpha(0.08f);
        }
    }

    
    void Update()
    {
        if (flashlightTransform == null || textRenderer == null) return;

        float distance = Vector3.Distance(flashlightTransform.position, transform.position);

        Vector3 directionToText = (transform.position - flashlightTransform.position).normalized;
        float angle = Vector3.Angle(flashlightTransform.forward, directionToText);

        if (distance < maxDistance && angle <= viewAngle)
        {
            float targerAlpha = 1f - (angle/viewAngle);
            UpdateMaterialAlpha(targerAlpha);
        }
        else
            UpdateMaterialAlpha(0.08f);
    }

    void UpdateMaterialAlpha(float targerAlpha)
    {
        Color color = textMaterial.color;
        color.a = Mathf.Lerp(color.a, targerAlpha, Time.deltaTime * 5f);
        textMaterial.color = color;
    }

    void SetTextAlpha(float alpha)
    {
        Color color = textMaterial.color;
        color.a = alpha;
        textMaterial.color = color;
    }
}
