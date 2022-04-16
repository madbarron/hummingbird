using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Animator>().Play("Drain", 0, 1 - (energy / maxEnergy));
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

    bool ITasty.IsTasty()
    {
        return energy > 0;
    }

    Vector3 ITasty.GetPosition()
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
            Debug.Log(energy.ToString() + ' ' + maxEnergy.ToString() + ' ' + normalizedEnergy.ToString());
        GetComponent<Animator>().Play("Drain", 0, normalizedEnergy);
        //GetComponent<Animator>().Play("Drain", 0, 0.99f);

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
