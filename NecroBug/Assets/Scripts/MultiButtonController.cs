using UnityEngine;
using System.Collections.Generic;

public class MultiButtonController : MonoBehaviour
{
    public List<GameObject> buttons;
    // particle system / explosion effect
    public float timerDuration = 20f;

    private HashSet<GameObject> pressedButtons = new HashSet<GameObject> ();
    private bool allButtonsPressed = false;
    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (allButtonsPressed){
            timer -= Time.deltaTime;
            // Debug.Log(timer); // a countdown
            if (timer <= 0f)
            {
                allButtonsPressed = false;
                Debug.Log("Timer has reached 0");
                // particle function
            }
        }
    }

    public void ButtonPressed(GameObject button)
    {
        if (!pressedButtons.Contains(button))
        {
            pressedButtons.Add(button);
        }

        if (pressedButtons.Count == buttons.Count && !allButtonsPressed)
        {
            StartTimer();
        }
    }

    private void StartTimer()
    {
        allButtonsPressed = true;
        timer = timerDuration;
        Debug.Log("counting down");
    }

}
