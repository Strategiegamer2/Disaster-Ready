using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultScreenButton : MonoBehaviour
{
    public void ReplayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
