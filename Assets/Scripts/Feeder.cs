using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Feeder : MonoBehaviour
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
        energy = Mathf.Clamp(energy - .1f * Time.deltaTime, 0, 1);
        GetComponent<Animator>().Play("Drain", 0, 1 - (energy / maxEnergy));
    }

}
