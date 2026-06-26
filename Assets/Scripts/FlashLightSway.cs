using UnityEngine;

public class FlashlightSway : MonoBehaviour
{
    public float amount = 0.05f;
    public float maxAmount = 0.1f;
    public float smoothAmount = 6f;

    private Vector3 initialPosition;

    void Start() => initialPosition = transform.localPosition;

    void Update()
    {
        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;

        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
    }
}