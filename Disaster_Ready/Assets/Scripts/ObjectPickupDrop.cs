using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupDropSystem : MonoBehaviour
{
    public float pickupRange = 3f;      // How close the player needs to be to pick up objects
    public Transform playerCamera;      // The camera or player's viewpoint
    public Transform holdPosition;      // The position where the object will be held in front of the player

    public GameObject pickupPromptUI;   // Reference to the UI for the pickup prompt
    public TextMeshProUGUI pickupText;             // Text element to display the pickup prompt

    private GameObject currentPickupObject = null; // The object the player is currently holding
    private GameObject detectedObject = null;      // Object currently detected for pickup
    private bool isHoldingObject = false;          // Whether the player is holding an object

    void Update()
    {
        // Check for objects with the "PickupDrop" tag in front of the player
        DetectPickupObject();

        // Check if the player presses the pickup/drop key
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHoldingObject)
            {
                DropObject();
            }
            else if (detectedObject != null)
            {
                TryPickupObject();
            }
        }

        // If the player is holding an object, update its position to stay in front of the player
        if (isHoldingObject && currentPickupObject != null)
        {
            HoldObject();
        }
    }

    // Detect objects with the "PickupDrop" tag within range
    void DetectPickupObject()
    {
        RaycastHit hit;

        // Cast a ray from the camera to detect objects with the "PickupDrop" tag
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("PickupDrop"))
            {
                detectedObject = hit.collider.gameObject;

                // Show the pickup prompt UI
                pickupPromptUI.SetActive(true);
                pickupText.text = "Press E to pick up " + detectedObject.name;
            }
            else
            {
                // Hide the pickup prompt if not looking at a valid object
                detectedObject = null;
                pickupPromptUI.SetActive(false);
            }
        }
        else
        {
            // Hide the pickup prompt if no object is detected
            detectedObject = null;
            pickupPromptUI.SetActive(false);
        }
    }

    // Try to pick up an object
    void TryPickupObject()
    {
        currentPickupObject = detectedObject;

        if (currentPickupObject != null)
        {
            // Pick up the object by setting it as a child of the hold position and disabling its physics
            currentPickupObject.transform.SetParent(holdPosition);
            currentPickupObject.transform.localPosition = Vector3.zero;
            currentPickupObject.transform.localRotation = Quaternion.identity;

            // Disable physics to prevent the object from falling
            Rigidbody rb = currentPickupObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;  // Disable rigidbody movement
            }

            isHoldingObject = true;

            // Hide the pickup prompt after picking up
            pickupPromptUI.SetActive(false);
        }
    }

    // Update the object's position to follow the player
    void HoldObject()
    {
        // Keep the object at the hold position in front of the player
        currentPickupObject.transform.position = holdPosition.position;
        currentPickupObject.transform.rotation = holdPosition.rotation;
    }

    // Drop the currently held object
    void DropObject()
    {
        if (currentPickupObject != null)
        {
            // Release the object by unparenting it
            currentPickupObject.transform.SetParent(null);

            // Re-enable physics
            Rigidbody rb = currentPickupObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;  // Re-enable rigidbody physics
                rb.AddForce(playerCamera.forward * 2f, ForceMode.Impulse); // Add a little push forward (optional)
            }

            isHoldingObject = false;
            currentPickupObject = null;
        }
    }
}
