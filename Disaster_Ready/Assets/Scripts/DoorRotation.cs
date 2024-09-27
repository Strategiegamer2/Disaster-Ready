using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotation : MonoBehaviour, IInteractable
{
    public Transform playerCamera;
    public float pickupRange = 3f;
    public float startYAxis;
    public float endYAxis;

    public bool isTheDoorOpen;

    void Update()
    {
        // Optionally, we could add other updates related to door animations here if needed.
    }

    // Implement the Interact method from the IInteractable interface
    public void Interact()
    {
        OpenDoor();
    }

    // Return the prompt text for the UIPromptController
    public string GetPromptText()
    {
        return isTheDoorOpen ? "Press E to close the door" : "Press E to open the door";
    }

    void OpenDoor()
    {
        if (!isTheDoorOpen)
        {
            // Open the door (rotate to end Y axis)
            iTween.RotateTo(this.gameObject, iTween.Hash("rotation", new Vector3(0, endYAxis, 0), "speed", 50f, "easetype", iTween.EaseType.easeOutQuart));
        }
        else
        {
            // Close the door (rotate back to start Y axis)
            iTween.RotateTo(this.gameObject, iTween.Hash("rotation", new Vector3(0, startYAxis, 0), "speed", 50f, "easetype", iTween.EaseType.easeOutQuart));
        }

        isTheDoorOpen = !isTheDoorOpen;
    }
}
