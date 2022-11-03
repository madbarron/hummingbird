using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A feeder has energy that the bird can consume over time.
/// </summary>
public class Feeder : MonoBehaviour, ITasty
{
    public float maxEnergy = 1;
    public float energy;
    public bool pollinatorMode = false;
    private DifficultyManager difficulty;

    // Start is called before the first frame update
    void Start()
    {
        difficulty = FindObjectOfType<DifficultyManager>();
        GetComponent<Animator>().speed = 0;

        applyDifficulty();
    }

    /// <summary>
    /// Drains energy from the feeder.
    /// </summary>
    /// <param name="requested"></param>
    /// <returns>Amount of energy actually drained</returns>
    public float DrainEnergy(float requested)
    {
        float delivered = Mathf.Clamp(requested, 0, energy);
        energy -= delivered;
        UpdateAnimation();
        return delivered;
    }

    /// <summary>
    /// Returns true if the bird wants to eat this
    /// </summary>
    /// <returns></returns>
    public bool IsTasty()
    {
        return energy > 0;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    protected void UpdateAnimation()
    {
        float normalizedEnergy = energy / maxEnergy;
        if (!pollinatorMode)
        {
            normalizedEnergy = 1 - normalizedEnergy;
        }
        GetComponent<Animator>().Play("Drain", 0, normalizedEnergy);
    }

    public void OnGameStart()
    {
        applyDifficulty();
    }

    protected void applyDifficulty()
    {
        maxEnergy = difficulty.Settings.flowerCapacity;
        pollinatorMode = difficulty.Settings.pollinatorMode;

        energy = maxEnergy;
        UpdateAnimation();
    }
}
