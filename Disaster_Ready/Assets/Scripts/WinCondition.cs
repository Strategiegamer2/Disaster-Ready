using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinCondition : MonoBehaviour
{
    // Reference to the parent GameObject (PickUp Items)
    public Transform pickUpItemsParent;

    // Optional UI Text to display the win message
    public TextMeshProUGUI winMessageText;

    // Time to wait before resetting the level (in seconds)
    public float resetDelay = 5f;

    private bool hasWon = false;  // To track if the win condition is met

    void Start()
    {
        // Hide the win message at the start
        if (winMessageText != null)
        {
            winMessageText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Check if the parent object has any children left and if the win condition hasn't been triggered yet
        if (pickUpItemsParent.childCount == 0 && !hasWon)
        {
            // Trigger the win condition if no children are left
            TriggerWinCondition();
        }
    }

    void TriggerWinCondition()
    {
        Debug.Log("You Win!");

        // If using a UI Text to show the win message
        if (winMessageText != null)
        {
            winMessageText.gameObject.SetActive(true);
            winMessageText.text = "You Win!";
        }

        // Set the win state to true
        hasWon = true;

        // Start the level reset after a delay
        Invoke("ResetLevel", resetDelay);
    }

    void ResetLevel()
    {
        Debug.Log("Resetting Level...");

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
