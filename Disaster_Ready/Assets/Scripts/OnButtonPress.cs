using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnButtonPress : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject controls;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex  + 1);
    }

    public void Options()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Controls()
    {
        mainMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void Back()
    {
        controls.SetActive(false);
        mainMenu.SetActive(true);
    }
}
