using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PickupSystem : MonoBehaviour
{
    // Variables for raycasting
    public float pickupRange = 3f;
    public Transform cameraTransform;

    // UI elements
    public GameObject pickupPromptUI;
    public TextMeshProUGUI pickupText;

    // Inventory UI images (these will be the slots)
    public List<Image> inventorySlots;  // Drag the images from Unity Inspector into this list
    public List<InventoryItem> itemDataList = new List<InventoryItem>(); // Holds data for each item

    // Inventory List to store picked-up items
    private List<InventoryItem> inventory = new List<InventoryItem>();

    // Reference to the current item we're looking at
    private GameObject currentPickupItem = null;

    // Object Placement
    private GameObject currentHologram = null;  // Hologram for placement
    private bool isPlacing = false;  // Flag to track if we're in placement mode
    private GameObject selectedBuildableItem = null;  // The object selected for placement (hologram)

    // Variables for Y-axis adjustment
    private float hologramYOffset = 0f;  // Y-offset for adjusting height
    private float scrollSensitivity = 0.5f;  // Sensitivity for mouse wheel scroll

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
        HandleRaycast();
        HandlePickup();
        HandleObjectPlacement();
        HandleInventoryToggle();
    }

    InventoryItem GetItemData(string itemName)
    {
        // Loop through itemDataList to find the matching item by name
        foreach (InventoryItem item in itemDataList)
        {
            if (item.itemName == itemName)
            {
                // Return the full InventoryItem object if the name matches
                return item;
            }
        }

        // If no match is found, return null (or handle this case differently if needed)
        return null;
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
                currentPickupItem = hit.collider.gameObject;

                // Show the pickup prompt
                pickupPromptUI.SetActive(true);
                pickupText.text = "Press E to pick up " + currentPickupItem.name;
            }
            else
            {
                currentPickupItem = null;
                pickupPromptUI.SetActive(false);
            }
        }
        else
        {
            currentPickupItem = null;
            pickupPromptUI.SetActive(false);
        }
    }

    // Handles picking up the item when the player presses E
    void HandlePickup()
    {
        if (currentPickupItem != null && Input.GetKeyDown(KeyCode.E))
        {
            // Get the item name from the picked-up object
            string pickedItemName = currentPickupItem.name;

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
                Destroy(currentPickupItem);

                // Hide the pickup prompt
                pickupPromptUI.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"Item '{pickedItemName}' not found in ItemDataList!");
            }
        }
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

                // Add a listener to detect when this slot is clicked
                int slotIndex = i;
                inventorySlots[i].GetComponent<Button>().onClick.AddListener(() => OnInventorySlotClicked(slotIndex));

                break; // Break after assigning to the first available slot
            }
        }
    }

    // When an inventory slot is clicked
    void OnInventorySlotClicked(int slotIndex)
    {
        // Get the inventory item associated with this slot
        InventoryItem selectedItem = itemDataList[slotIndex];

        // Check if the item is buildable
        if (selectedItem.isBuildable)
        {
            StartPlacementMode(selectedItem);
        }
    }

    // Start the object placement mode
    void StartPlacementMode(InventoryItem item)
    {
        // Exit time stop by resuming time
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Enter placement mode
        isPlacing = true;

        // Get the prefab for the selected buildable item (hologram)
        selectedBuildableItem = item.buildablePrefab;

        // Instantiate the hologram (temporary transparent model) at the player's position
        if (currentHologram == null && selectedBuildableItem != null)
        {
            currentHologram = Instantiate(selectedBuildableItem);
            // Optional: Set the material or shader to give it a "hologram" appearance
            SetHologramAppearance(currentHologram);
        }

        // Reset the Y-offset when entering build mode
        hologramYOffset = 0f;
    }

    // Handle object placement (move hologram and place the object)
    void HandleObjectPlacement()
    {
        if (isPlacing && currentHologram != null)
        {
            RaycastHit hit;

            // Raycast from the camera to the ground (or surfaces) to move the hologram
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                // Move the hologram to the hit point with an adjusted Y position
                currentHologram.transform.position = new Vector3(hit.point.x, hit.point.y + hologramYOffset, hit.point.z);

                // Rotate the hologram to match the ground normal (optional, if needed)
                currentHologram.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }

            // Adjust the Y position using the mouse wheel
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                hologramYOffset += scrollInput * scrollSensitivity;
            }

            // Place the object when the left mouse button is pressed
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                PlaceObject(currentHologram.transform.position);
            }
        }
    }

    // Place the object on the ground
    void PlaceObject(Vector3 position)
    {
        if (currentHologram != null)
        {
            // Instantiate the **final prefab** from the item data at the hologram's position
            if (itemDataList != null)
            {
                InventoryItem selectedItem = itemDataList.Find(item => item.buildablePrefab == selectedBuildableItem);
                if (selectedItem != null && selectedItem.finalPrefab != null)
                {
                    Instantiate(selectedItem.finalPrefab, currentHologram.transform.position, currentHologram.transform.rotation);
                }
                else
                {
                    Debug.LogWarning("Final prefab is not assigned in ItemDataList!");
                }
            }

            // Destroy the hologram
            Destroy(currentHologram);

            // Exit placement mode
            isPlacing = false;
            selectedBuildableItem = null;
            currentHologram = null;
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

    // Optional: Set a "hologram" appearance for the object
    void SetHologramAppearance(GameObject hologram)
    {
        // Modify the material or shader here to make the object look like a hologram
        // This is optional, depending on how you want the hologram to appear
        Renderer renderer = hologram.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Example: make the material semi-transparent
            renderer.material.color = new Color(0, 1, 0, 0.5f); // Green with 50% transparency
        }
    }
}
