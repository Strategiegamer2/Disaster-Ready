using UnityEngine;
using TMPro;

public class DisplayResults : MonoBehaviour
{
    public TextMeshProUGUI pointsText;      // UI element to display total points
    public TextMeshProUGUI task1Text;       // UI element to display Task 1 completion
    public TextMeshProUGUI task2Text;       // UI element to display Task 2 completion
    public TextMeshProUGUI task1ExplanationText;  // UI element for Task 1 explanation
    public TextMeshProUGUI task2ExplanationText;  // UI element for Task 2 explanation

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Get the player's total points and task completion statuses from PlayerPrefs
        int totalPoints = PlayerPrefs.GetInt("TotalPoints", 0);
        int task1Complete = PlayerPrefs.GetInt("Task1Complete", 0);
        int task2Complete = PlayerPrefs.GetInt("Task2Complete", 0);

        // Display total points
        pointsText.text = "Total Points: " + totalPoints;

        // Display task completion statuses and explanations
        if (task1Complete == 1)
        {
            task1Text.text = "Gather resources: Completed";
            task1ExplanationText.text = "Task 1: You have gathered all the necessary supplies and food, ensuring survival in the case of emergencies.";
        }
        else
        {
            task1Text.text = "Gather resources: Not Completed";
            task1ExplanationText.text = "Task 1: It is important to gather food and supplies, or already have a kit ready. You never know how long rescue can take.";
        }

        if (task2Complete == 1)
        {
            task2Text.text = "Place sandbags by the door: Completed";
            task2ExplanationText.text = "Task 2: You have prepared your home for the coming flood, reducing the risk of damage.";
        }
        else
        {
            task2Text.text = "Place sandbags by the door: Not Completed";
            task2ExplanationText.text = "Task 2: If you know a flood is coming, it is always good to do some preparations to protect your home. This can prevent damage.";
        }
    }
}
