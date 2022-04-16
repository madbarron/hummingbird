using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityGameEventListener : GameEventListener
{
    public UnityEvent Response;

    public override void OnEventRaised()
    {
        Response.Invoke();
    }
}