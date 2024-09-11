using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowTimer : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] MoveObject timer;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        int timeLeft = (int)timer.timer;
        text.text = "Time Left: " + timeLeft;
    }
}
