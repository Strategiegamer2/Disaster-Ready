using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    public WinCondition winCondition;  // Reference to the WinCondition script

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player"))  // Ensure the player has the "Player" tag
        {
            Debug.Log("Player entered the trigger, ending the game.");
            winCondition.EndGame();  // End the game and go to the results scene
        }
    }
}
