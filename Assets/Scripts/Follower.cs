using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform followTarget;
    public bool followX = true;
    public bool followY = true;
    public float scale = 1;

    private Vector3 targetLastPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetLastPosition = followTarget.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 shift = followTarget.position - targetLastPosition;

        shift *= scale;

        float x = followX ? shift.x : 0;
        float y = followY ? shift.y : 0;

        transform.position += new Vector3(x, y, 0);
        targetLastPosition = followTarget.position;
    }
}
