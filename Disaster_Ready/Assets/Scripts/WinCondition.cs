using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinCondition : MonoBehaviour
{
    // Reference to the parent GameObject for PickUp Items for Task 1
    public Transform pickUpItemsParent;

    // Reference to the parent GameObject for all doors and windows (for Task 3)
    public Transform doorsAndWindowsParent;

    // Points for each task
    public int task1Points = 50;
    public int task2Points = 50;
    public int task3Points = 50;

    private bool task1Complete = false;
    private bool task2Complete = false;
    private bool task3Complete = false;
    private int totalPoints = 0;

    void Update()
    {
        // Check if Task 1 (collecting all PickUp Items) is complete
        if (pickUpItemsParent.childCount == 0 && !task1Complete)
        {
            CompleteTask1();
        }

        // Check if Task 3 (all doors/windows back to start position) is complete
        if (!task3Complete && AreAllDoorsAndWindowsClosed())
        {
            CompleteTask3();
        }
    }

    // Task 1: Collect all PickUp Items
    void CompleteTask1()
    {
        task1Complete = true;
        totalPoints += task1Points;
        Debug.Log("Task 1 complete! Points: " + task1Points);
    }

    // Task 2: Set from the PickupDropTrigger script
    public void CompleteTask2()
    {
        if (!task2Complete)
        {
            task2Complete = true;
            totalPoints += task2Points;
            Debug.Log("Task 2 complete! Points: " + task2Points);
        }
    }

    // Task 3: Check if all doors/windows are at their startYAxis
    void CompleteTask3()
    {
        task3Complete = true;
        totalPoints += task3Points;
        Debug.Log("Task 3 complete! Points: " + task3Points);
    }

    // Transition to the results scene
    public void EndGame()
    {
        // Store points and task completion in PlayerPrefs
        PlayerPrefs.SetInt("TotalPoints", totalPoints);
        PlayerPrefs.SetInt("Task1Complete", task1Complete ? 1 : 0);
        PlayerPrefs.SetInt("Task2Complete", task2Complete ? 1 : 0);
        PlayerPrefs.SetInt("Task3Complete", task3Complete ? 1 : 0);

        // Load the results screen
        SceneManager.LoadScene("ResultsScene");  // Replace with your results scene name
    }

    bool AreAllDoorsAndWindowsClosed()
    {
        foreach (Transform child in doorsAndWindowsParent)
        {
            // Check only immediate children
            DoorRotation doorRotation = child.GetComponent<DoorRotation>();
            if (doorRotation != null)
            {
                // Get the current Y rotation of the door/window (local Y axis)
                float currentYRotation = child.localEulerAngles.y;

                // Compare with the start Y axis using a small tolerance
                if (Mathf.Abs(currentYRotation - doorRotation.startYAxis) > 0.1f)
                {
                    return false;  // If any door/window is not within tolerance of the startYAxis, task is not complete
                }
            }
        }

        return true; // All doors/windows are at the startYAxis
    }

}
