using UnityEngine;
using TMPro;

public class UIPromptController : MonoBehaviour
{
    public float pickupRange = 3f;                   // How far the player can interact
    public Transform playerCamera;                   // The player's camera
    public GameObject pickupPromptUI;                // UI for the prompt
    public TextMeshProUGUI pickupText;               // Text for the prompt
    public LayerMask interactableLayers;             // LayerMask for interactable objects

    private GameObject detectedObject = null;        // Object currently detected for interaction
    private IInteractable interactable = null;       // Reference to an interactable object interface

    void Update()
    {
        DetectInteractableObject();

        // If a valid interactable object is detected, show the UI and interact on keypress
        if (detectedObject != null && Input.GetKeyDown(KeyCode.E))
        {
            interactable?.Interact(); // Call the Interact method if the object implements IInteractable
        }
    }

    void DetectInteractableObject()
    {
        RaycastHit hit;

        // Cast a ray from the camera to detect objects within the interactable layers
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, pickupRange, interactableLayers))
        {
            detectedObject = hit.collider.gameObject;
            interactable = detectedObject.GetComponent<IInteractable>();

            // Show the appropriate prompt text if the object is interactable
            if (interactable != null)
            {
                pickupPromptUI.SetActive(true);
                pickupText.text = interactable.GetPromptText();
            }
        }
        else
        {
            // Hide the prompt if no object is detected
            detectedObject = null;
            interactable = null;
            pickupPromptUI.SetActive(false);
        }
    }
}
