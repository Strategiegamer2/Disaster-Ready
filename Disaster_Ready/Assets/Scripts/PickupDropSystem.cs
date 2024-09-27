using UnityEngine;

public class PickupDropSystem : MonoBehaviour, IInteractable
{
    public Transform playerCamera; // The camera or player's viewpoint
    public Transform holdPosition; // The position where the object will be held in front of the player
    private GameObject currentPickupObject = null;
    private bool isHoldingObject = false;

    public void Interact()
    {
        Debug.Log("PickupDropSystem: Interact called.");
        if (isHoldingObject)
        {
            DropObject();
        }
        else
        {
            TryPickupObject();
        }
    }

    public string GetPromptText()
    {
        return isHoldingObject ? "Press E to drop " + currentPickupObject?.name : "Press E to pick up " + gameObject.name;
    }

    private void TryPickupObject()
    {
        currentPickupObject = gameObject;
        if (currentPickupObject != null)
        {
            currentPickupObject.transform.SetParent(holdPosition);
            currentPickupObject.transform.localPosition = Vector3.zero;
            currentPickupObject.transform.localRotation = Quaternion.identity;

            Rigidbody rb = currentPickupObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            isHoldingObject = true;
        }
    }

    private void DropObject()
    {
        if (currentPickupObject != null)
        {
            currentPickupObject.transform.SetParent(null);

            Rigidbody rb = currentPickupObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(playerCamera.forward * 2f, ForceMode.Impulse); // Optional: push forward
            }

            isHoldingObject = false;
            currentPickupObject = null;
        }
    }
}
