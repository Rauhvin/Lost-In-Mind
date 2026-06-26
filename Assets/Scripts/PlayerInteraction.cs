using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Input")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private Camera mainCamera;
    private bool lookedAtFridgeLastFrame = false; 

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        CheckRaycast();
    }

    private void CheckRaycast()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            
            InteractableDoor fridgeDoor = hit.collider.GetComponent<InteractableDoor>();

            if (fridgeDoor != null)
            {
                lookedAtFridgeLastFrame = true;
              
                UIManager.Instance.ShowText("Press E, to use", this);

                if (Input.GetKeyDown(interactionKey))
                {
                    fridgeDoor.Interact();
                }
                return;
            }

            
            if (lookedAtFridgeLastFrame)
            {
                
                UIManager.Instance.HideText(this);
                lookedAtFridgeLastFrame = false;
            }
        }
        else
        {
            
            if (lookedAtFridgeLastFrame)
            {
                
                UIManager.Instance.HideText(this);
                lookedAtFridgeLastFrame = false;
            }
        }
    }
}