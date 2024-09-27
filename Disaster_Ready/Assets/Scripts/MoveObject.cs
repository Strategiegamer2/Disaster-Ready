using UnityEngine;

public class MoveObject : MonoBehaviour
{
    Rigidbody rb;

    public float timer = 120f;               // Total time before the final move happens
    public float secondTargetY = 5f;         // The height the water will rise to at 40 seconds
    public float secondTargetTime = 40f;     // Time when the water should rise to secondTargetY
    public float targetY;                    // Final water level when the timer reaches 0
    public float moveSpeed = 0.05f;          // Speed at which the water rises

    private bool movingToSecondLevel = false; // Track if the water is moving to the intermediate level
    private bool movingToFinalLevel = false;  // Track if the water is moving to the final level

    public PickupDropTrigger pickupDropTrigger;  // Reference to PickupDropTrigger

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        // Ensure that Rigidbody is set
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found!");
        }
    }

    void FixedUpdate()
    {
        // Decrease the timer if it's above 0
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            // Start moving water to the intermediate height at 40 seconds if fewer than 3 objects are in the trigger
            if (!movingToSecondLevel && timer <= secondTargetTime && pickupDropTrigger.ObjectsInTrigger < 3)
            {
                movingToSecondLevel = true;
            }
        }

        // If timer reaches 0, start moving to the final targetY
        if (timer <= 0 && !movingToFinalLevel)
        {
            movingToFinalLevel = true;
        }

        // Move the water if we're supposed to go to the second level
        if (movingToSecondLevel)
        {
            MoveWater(secondTargetY);  // Smoothly move to intermediate level
        }

        // Move the water to the final level once the timer reaches 0
        if (movingToFinalLevel)
        {
            MoveWater(targetY);  // Smoothly move to final level
        }
    }

    // Function to move the water smoothly towards a target Y level
    void MoveWater(float targetLevel)
    {
        // Calculate the new Y position using Mathf.MoveTowards for smooth movement
        float newY = Mathf.MoveTowards(transform.position.y, targetLevel, moveSpeed * Time.deltaTime);

        // Move the Rigidbody to the new position
        rb.MovePosition(new Vector3(transform.position.x, newY, transform.position.z));

        // Log the position to debug
        Debug.Log("Water moving towards: " + targetLevel + ", Current Y: " + transform.position.y);

        // If we've reached the target level, stop moving
        if (Mathf.Abs(transform.position.y - targetLevel) < 0.01f)
        {
            Debug.Log("Reached target level: " + targetLevel);

            if (targetLevel == secondTargetY)
            {
                movingToSecondLevel = false;  // Stop moving to the second level
            }

            if (targetLevel == targetY)
            {
                movingToFinalLevel = false;   // Stop moving to the final level
            }
        }
    }
}
