using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CabinetMovement : MonoBehaviour
{
    public Transform playerCamera;
    public float pickupRange = 3f;

    public GameObject pickupPromptUI;
    public TextMeshProUGUI pickupText;

    public bool isTheCabinetOpen;
    public float cabinetOpenDistance = 2f;  // The distance the cabinet moves when opened

    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        // Store the initial position as the closed position
        closedPosition = transform.position;
        // Calculate the open position by moving the cabinet along the Z-axis
        openPosition = closedPosition + new Vector3(0, 0, cabinetOpenDistance);

        pickupPromptUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckCabinetInRange();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsCabinetInRange())
            {
                MoveCabinet();
            }
        }
    }

    void CheckCabinetInRange()
    {
        if (IsCabinetInRange())
        {
            pickupPromptUI.SetActive(true);
            pickupText.text = isTheCabinetOpen ? "Press E to Close the Cabinet" : "Press E to Open the Cabinet";
        }
        else
        {
            pickupPromptUI.SetActive(false);
        }
    }

    bool IsCabinetInRange()
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

    void MoveCabinet()
    {
        if (!isTheCabinetOpen)
        {
            // Move the cabinet to the open position
            iTween.MoveTo(this.gameObject, iTween.Hash("position", openPosition, "speed", 2f, "easetype", iTween.EaseType.easeOutQuart));
        }
        else
        {
            // Move the cabinet back to the closed position
            iTween.MoveTo(this.gameObject, iTween.Hash("position", closedPosition, "speed", 2f, "easetype", iTween.EaseType.easeOutQuart));
        }

        isTheCabinetOpen = !isTheCabinetOpen;
    }
}
