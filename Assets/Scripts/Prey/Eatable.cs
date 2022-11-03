using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents something that can be eaten immediately for a point
/// </summary>
public class Eatable : MonoBehaviour, ITasty
{
    // Eatables are deleted when they are eaten
    public bool IsTasty()
    {
        return true;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
