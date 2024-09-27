using UnityEngine;

public class PickupDropTrigger : MonoBehaviour
{
    public WinCondition winCondition;  // Reference to the WinCondition script
    private int objectsInTrigger = 0;  // Counter for objects with "PickupDrop" tag inside the trigger

    // Getter for objects in the trigger
    public int ObjectsInTrigger => objectsInTrigger;

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the "PickupDrop" tag
        if (other.CompareTag("PickupDrop"))
        {
            objectsInTrigger++;
            Debug.Log("Object entered: " + objectsInTrigger + " objects inside the trigger.");

            // If exactly 3 objects are inside the trigger, complete Task 2
            if (objectsInTrigger == 3)
            {
                winCondition.CompleteTask2();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object that left the trigger has the "PickupDrop" tag
        if (other.CompareTag("PickupDrop"))
        {
            objectsInTrigger--;
            Debug.Log("Object exited: " + objectsInTrigger + " objects inside the trigger.");
        }
    }
}
