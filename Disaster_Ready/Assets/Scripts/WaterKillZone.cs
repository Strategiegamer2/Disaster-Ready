using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterKillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        Debug.Log("Player died in water!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
