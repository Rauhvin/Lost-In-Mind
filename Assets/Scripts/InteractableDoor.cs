using System.Collections;
using UnityEngine;

public class InteractableDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float openAngle = 90f;   
    [SerializeField] private float openSpeed = 3f;    
    [SerializeField] private bool isOpen = false;     

    [Header("Locked Content (Optional)")]
    
    [SerializeField] private GameObject itemInside;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine movementCoroutine;

    void Start()
    {
        
        closedRotation = transform.localRotation;

        
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);

        if (isOpen)
        {
            transform.localRotation = openRotation;
            
            SetItemActiveState(true);
        }
        else
        {
            
            SetItemActiveState(false);
        }
    }

    public void Interact()
    {
        isOpen = !isOpen;

        
        SetItemActiveState(isOpen);

        
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        movementCoroutine = StartCoroutine(AnimateDoor(targetRotation));
    }

    private void SetItemActiveState(bool state)
    {
        if (itemInside != null)
        {
            
            InventoryItem itemScript = itemInside.GetComponent<InventoryItem>();

            if (itemScript != null)
            {
                itemScript.canBePickedUp = state;

                if (!state)
                {
                    //UIManager.Instance.HideText();
                }
            }
        }
    }

    private IEnumerator AnimateDoor(Quaternion target)
    {
        while (Quaternion.Angle(transform.localRotation, target) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                target,
                Time.deltaTime * openSpeed
            );
            yield return null;
        }

        transform.localRotation = target;
        movementCoroutine = null;
    }
}