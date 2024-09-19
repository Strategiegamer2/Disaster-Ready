using UnityEngine;
using TMPro;

public class DisplayResults : MonoBehaviour
{
    public TextMeshProUGUI pointsText;      // UI element to display points
    public TextMeshProUGUI task1Text;       // UI element to display Task 1 completion
    public TextMeshProUGUI task2Text;       // UI element to display Task 2 completion

    void Start()
    {
        // Get the player's total points and task completion statuses from PlayerPrefs
        int totalPoints = PlayerPrefs.GetInt("TotalPoints", 0);
        int task1Complete = PlayerPrefs.GetInt("Task1Complete", 0);
        int task2Complete = PlayerPrefs.GetInt("Task2Complete", 0);

        // Display total points
        pointsText.text = "Total Points: " + totalPoints;

        // Display task completion statuses
        task1Text.text = "Task 1: " + (task1Complete == 1 ? "Completed" : "Not Completed");
        task2Text.text = "Task 2: " + (task2Complete == 1 ? "Completed" : "Not Completed");
    }
}
