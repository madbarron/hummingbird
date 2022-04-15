using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Feeder : MonoBehaviour, ITasty
{
    public float maxEnergy = 1;
    public float energy;

    // Start is called before the first frame update
    void Start()
    {
        energy = maxEnergy;
        GetComponent<Animator>().speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //energy = Mathf.Clamp(energy - .1f * Time.deltaTime, 0, 1);
        GetComponent<Animator>().Play("Drain", 0, 1 - (energy / maxEnergy));
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
        GetComponent<Animator>().Play("Drain", 0, 1 - (energy / maxEnergy));
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

}
