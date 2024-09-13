using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;           // Name of the item
    public Sprite itemIcon;           // Icon to display in inventory
    public GameObject buildablePrefab; // Hologram prefab for placement
    public GameObject finalPrefab;    // Final prefab to instantiate on placement
    public bool isBuildable;          // Is this item buildable?

    public InventoryItem(string name, Sprite icon, GameObject buildable, GameObject final, bool buildableFlag)
    {
        itemName = name;
        itemIcon = icon;
        buildablePrefab = buildable;
        finalPrefab = final;
        isBuildable = buildableFlag;
    }
}

