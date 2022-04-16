using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventListener : MonoBehaviour
{
    public GameEvent Event;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public abstract void OnEventRaised();
}
