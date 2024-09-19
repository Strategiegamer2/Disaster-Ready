using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinCondition : MonoBehaviour
{
    // Reference to the parent GameObject (PickUp Items for Task 1)
    public Transform pickUpItemsParent;

    // Points for each task
    public int task1Points = 50;
    public int task2Points = 50;

    private bool task1Complete = false;
    private bool task2Complete = false;
    private int totalPoints = 0;

    void Update()
    {
        // Check if Task 1 (collecting all PickUp Items) is complete
        if (pickUpItemsParent.childCount == 0 && !task1Complete)
        {
            CompleteTask1();
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

    // Transition to the results scene
    public void EndGame()
    {
        // Store points and task completion in PlayerPrefs
        PlayerPrefs.SetInt("TotalPoints", totalPoints);
        PlayerPrefs.SetInt("Task1Complete", task1Complete ? 1 : 0);
        PlayerPrefs.SetInt("Task2Complete", task2Complete ? 1 : 0);

        // Load the results screen
        SceneManager.LoadScene("ResultsScene");  // Replace with your results scene name
    }
}
