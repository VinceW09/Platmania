using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerEndUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    void Update()
    {
        timerText.text = "YOUR TIME: " + Timer.Instance.GetThisTimeAsString();
    }
}
