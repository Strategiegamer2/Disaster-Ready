using UnityEngine;
using UnityEngine.UI; // For UI elements
using System.Collections.Generic; // For the inventory system
using TMPro;

public class PickupSystem : MonoBehaviour
{
    // Variables for raycasting
    public float pickupRange = 3f;
    public Transform cameraTransform;

    // UI prompt for picking up items
    public GameObject pickupPromptUI;
    public TextMeshProUGUI pickupText; // The text element that says "Press E to pick up"

    // Inventory
    private List<string> inventory = new List<string>();

    // Reference to the current item we are looking at
    private GameObject currentPickupItem = null;

    void Update()
    {
        HandleRaycast();
        HandlePickup();
    }

    // Handles the raycasting to detect objects with the "Pickup" tag
    void HandleRaycast()
    {
        RaycastHit hit;

        // Cast a ray from the camera forward
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, pickupRange))
        {
            // Check if the object hit has the tag "Pickup"
            if (hit.collider.CompareTag("Pickup"))
            {
                // Get the object we hit
                currentPickupItem = hit.collider.gameObject;

                // Show the UI prompt and update the text
                pickupPromptUI.SetActive(true);
                pickupText.text = "Press E to pick up " + currentPickupItem.name;
            }
            else
            {
                // Hide the UI if not looking at a pickup object
                currentPickupItem = null;
                pickupPromptUI.SetActive(false);
            }
        }
        else
        {
            // Hide the UI if the raycast doesn't hit anything
            currentPickupItem = null;
            pickupPromptUI.SetActive(false);
        }
    }

    // Handles picking up the item when the player presses E
    void HandlePickup()
    {
        // Check if we have a valid pickup object and if the player presses E
        if (currentPickupItem != null && Input.GetKeyDown(KeyCode.E))
        {
            // Add the item to the inventory
            AddToInventory(currentPickupItem.name);

            // Destroy or deactivate the object (optional)
            Destroy(currentPickupItem);

            // Hide the UI prompt
            pickupPromptUI.SetActive(false);
        }
    }

    // Add the item to the player's inventory
    void AddToInventory(string itemName)
    {
        inventory.Add(itemName);
        Debug.Log("Picked up: " + itemName);
        Debug.Log("Current Inventory: " + string.Join(", ", inventory));
    }
}
