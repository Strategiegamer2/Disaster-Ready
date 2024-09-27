using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    // UI elements for the pause menu and settings
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    // Mouse sensitivity slider and TextMeshPro display
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityText;
    private PlayerMovement playerMovementScript;

    // The actual sensitivity range (100 to 300)
    public float minSensitivity = 20f;
    public float maxSensitivity = 500f;

    // The key used to save/load sensitivity in PlayerPrefs
    private const string MouseSensitivityKey = "MouseSensitivity";

    void Start()
    {
        // Reference the PlayerMovement script from your player object
        playerMovementScript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        // Load the saved sensitivity from PlayerPrefs (or use default if not set)
        float savedSensitivity = PlayerPrefs.GetFloat(MouseSensitivityKey, 150f); // Default to 150 if no value exists
        playerMovementScript.MouseSensitivity = savedSensitivity;

        // Set the slider value to match the loaded sensitivity
        sensitivitySlider.value = savedSensitivity;

        // Update the sensitivity text to reflect the loaded sensitivity
        UpdateSensitivityText(savedSensitivity);

        // Disable pause and settings menus at the start
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    // Adjust mouse sensitivity based on the slider value and save the new value
    public void AdjustSensitivity()
    {
        // Apply the slider value directly to the player's mouse sensitivity
        float newSensitivity = sensitivitySlider.value;
        playerMovementScript.MouseSensitivity = newSensitivity;

        // Save the new sensitivity to PlayerPrefs
        PlayerPrefs.SetFloat(MouseSensitivityKey, newSensitivity);
        PlayerPrefs.Save(); // Ensure the value is saved to disk

        // Update the sensitivity text to reflect the new value
        UpdateSensitivityText(newSensitivity);
    }

    // Update the sensitivity text with a value mapped from 100-300 to 1-100
    void UpdateSensitivityText(float currentSensitivity)
    {
        // Convert the current sensitivity from the range (100-300) to a 1-100 scale
        float sensitivityValue = Mathf.InverseLerp(minSensitivity, maxSensitivity, currentSensitivity) * 100;
        sensitivityText.text = "Sensitivity: " + Mathf.RoundToInt(sensitivityValue).ToString();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting...");
    }
}
