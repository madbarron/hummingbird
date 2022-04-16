using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DifficultySettings : ScriptableObject
{
    public bool godMode;

    public float drinkRate;
    public float flowerCapacity;
    public float healthPowerDrain;

    public float massGainPerScore;
}
