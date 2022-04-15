using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eatable : MonoBehaviour, ITasty
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsTasty()
    {
        return true;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
