using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PickupSystem : MonoBehaviour
{
    public float pickupRange = 3f;          // How far the player can interact with items
    public Transform cameraTransform;       // The player's camera for raycasting
    public LayerMask interactableLayers;    // Layers that include interactable items (Pickup Layer)

    // Inventory UI images (these will be the slots)
    public List<Image> inventorySlots;      // Drag the images from Unity Inspector into this list
    public List<InventoryItem> itemDataList = new List<InventoryItem>(); // Holds data for each item

    // Inventory List to store picked-up items
    private List<InventoryItem> inventory = new List<InventoryItem>();

    // Reference to the current item we're looking at
    private GameObject currentPickupItem = null;

    void Start()
    {
        // Initially hide all inventory slots
        foreach (var slot in inventorySlots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // This handles any inventory update or placement mode
        HandleInventoryToggle();

        // Handle raycasting to detect pickup items
        DetectPickupItem();

        // Handle picking up the item when the player presses E
        if (currentPickupItem != null && Input.GetKeyDown(KeyCode.E))
        {
            HandlePickup(currentPickupItem);
        }
    }

    void DetectPickupItem()
    {
        RaycastHit hit;

        // Cast a ray from the camera to detect objects within the interactable layers
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, pickupRange, interactableLayers))
        {
            currentPickupItem = hit.collider.gameObject;  // The detected item

            // Check if the item has a name that corresponds to an InventoryItem
            InventoryItem itemData = GetItemData(currentPickupItem.name);
            if (itemData != null)
            {
                // Show prompt if item is interactable (this can be handled in UIPromptController)
                Debug.Log("Press E to pick up " + currentPickupItem.name);
            }
        }
        else
        {
            currentPickupItem = null;  // No item detected
        }
    }


    // Handles picking up the item when the player presses E
    void HandlePickup(GameObject pickupItem)
    {
        // Get the item name from the picked-up object
        string pickedItemName = pickupItem.name;

        // Retrieve the full item data from itemDataList based on the item name
        InventoryItem pickedItemData = GetItemData(pickedItemName);

        // Ensure that the item exists in the itemDataList
        if (pickedItemData != null)
        {
            // Add this item to the inventory
            AddToInventory(pickedItemData);

            // Update the inventory UI with the new item
            UpdateInventoryUI(pickedItemData);

            // Destroy or deactivate the picked-up object
            Destroy(pickupItem);
        }
        else
        {
            Debug.LogWarning($"Item '{pickedItemName}' not found in ItemDataList!");
        }
    }

    // Return the prompt text for the UIPromptController (if needed)
    public string GetPromptText()
    {
        return currentPickupItem != null ? "Press E to pick up " + currentPickupItem.name : "";
    }

    // Helper method to get item data by name
    InventoryItem GetItemData(string itemName)
    {
        foreach (InventoryItem item in itemDataList)
        {
            if (item.itemName == itemName)
            {
                return item;
            }
        }

        return null;
    }

    // Add the item to the inventory list
    void AddToInventory(InventoryItem item)
    {
        inventory.Add(item);
        Debug.Log("Picked up: " + item.itemName);
    }

    // Update the inventory UI when an item is picked up
    void UpdateInventoryUI(InventoryItem item)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            // Find the first empty slot
            if (!inventorySlots[i].gameObject.activeSelf)
            {
                // Activate the slot and set the correct sprite (item icon)
                inventorySlots[i].gameObject.SetActive(true);
                inventorySlots[i].sprite = item.itemIcon; // Use the icon from item data

                // Store the full item data in the itemDataList for this slot
                itemDataList[i] = item;

                break; // Break after assigning to the first available slot
            }
        }
    }

    // Handles opening/closing the inventory (optional for toggling visibility)
    void HandleInventoryToggle()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isActive = inventorySlots[0].transform.parent.gameObject.activeSelf;
            inventorySlots[0].transform.parent.gameObject.SetActive(!isActive);

            if (!isActive)
            {
                // Pause the game if inventory is open
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Resume the game when inventory is closed
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
