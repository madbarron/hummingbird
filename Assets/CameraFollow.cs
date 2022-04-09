using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public bool followX = true;
    public bool followY = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = followX ? followTarget.position.x : transform.position.x;
        float y = followY ? followTarget.position.y : transform.position.y;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
