using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Hotkeys : MonoBehaviour
{
    public UnityEvent onRestart;
    public UnityEvent onMute;
    public UnityEvent onUnMute;

    bool mute = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            onRestart?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mute)
            {
                onUnMute?.Invoke();
            }
            else
            {
                onMute?.Invoke();
            }

            mute = !mute;
        }
    }
}
