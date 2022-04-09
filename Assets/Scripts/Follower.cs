using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform followTarget;
    public bool followX = true;
    public bool followY = true;
    public float scale = 1;

    private Vector3 startPosition;
    private Vector3 targetStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        targetStartPosition = followTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 shift = followTarget.position - targetStartPosition;

        shift *= scale;

        float x = followX ? shift.x : 0;
        float y = followY ? shift.y : 0;

        transform.position = startPosition + new Vector3(x, y, 0);
    }
}
