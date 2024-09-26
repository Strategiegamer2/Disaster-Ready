using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorRotation : MonoBehaviour
{
    public Transform playerCamera;
    public float pickupRange = 3f;
    public float startYAxis;
    public float endYAxis;

    public GameObject pickupPromptUI;
    public TextMeshProUGUI pickupText;

    public bool isTheDoorOpen;

    void Start()
    {
        pickupPromptUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckDoorInRange();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsDoorInRange())
            {
                OpenDoor();
            }
        }
    }

    void CheckDoorInRange()
    {
        if (IsDoorInRange())
        {
            pickupPromptUI.SetActive(true);
            pickupText.text = isTheDoorOpen ? "Press E to Close the Door" : "Press E to Open the Door";
        }
        else
        {
            pickupPromptUI.SetActive(false);
        }
    }

    bool IsDoorInRange()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, pickupRange))
        {
            if (hit.transform == this.transform)
            {
                return true;
            }
        }
        return false;
    }

    void OpenDoor()
    {
        if (!isTheDoorOpen)
        {
            // Open the door (rotate to -90 degrees)
            iTween.RotateTo(this.gameObject, iTween.Hash("rotation", new Vector3(0, endYAxis, 0), "speed", 50f, "easetype", iTween.EaseType.easeOutQuart));
        }
        else
        {
            // Close the door (rotate back to 0 degrees)
            iTween.RotateTo(this.gameObject, iTween.Hash("rotation", new Vector3(0, startYAxis, 0), "speed", 50f, "easetype", iTween.EaseType.easeOutQuart));
        }

        isTheDoorOpen = !isTheDoorOpen;
    }
}
