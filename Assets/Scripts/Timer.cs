using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class Timer : MonoBehaviour
{
    // The TextMeshProUGUI component used to display the timer
    [Header("Component")]
    public TextMeshProUGUI timerText;

    // The current time of the timer
    [Header("Timer Settings")]
    public float currentTime;
    public bool isCounting;

    // Whether the timer should count down or up
    public bool countDown;

    // Whether the timer has a time limit
    [Header("Limit Settings")]
    public bool hasLimit;

    // The time limit of the timer (only used if hasLimit is true)
    public float timerLimit;

    private void Update()
    {
        // Update the current time based on whether the timer is counting up or down
        if (countDown && isCounting)
        {
            currentTime -= Time.deltaTime;
        }
        else if (!countDown && isCounting)
        {
            currentTime += Time.deltaTime;
        }

        // If the timer has a limit and that limit has been reached, stop the timer and change its color to red
        if (hasLimit && ((countDown && currentTime <= timerLimit) || (!countDown && currentTime >= timerLimit)))
        {
            // Set the current time to the timer limit and update the timer text to reflect this
            currentTime = timerLimit;
            SetTimerText();

            // Change the color of the timer text to red and disable the Timer component so it won't update anymore
            timerText.color = Color.red;
            enabled = false;
        }

        // Update the timer text with the current time
        SetTimerText();
    }

    // Update the timer text with the current time
    private void SetTimerText()
    {
        timerText.text = currentTime.ToString("0.00");
    }

    public void ResetTimer()
    {
        currentTime = 0f;
    }
}
