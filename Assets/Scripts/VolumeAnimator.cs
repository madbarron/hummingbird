using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Animations;

[RequireComponent(typeof(Volume))]
public class VolumeAnimator : MonoBehaviour
{
    [SerializeField]
    Volume volume;

    [SerializeField]
    AnimationCurve focalLength;

    DepthOfField depthOfField;

    public float Time = 0;
    //public float Time { set { time = value; } }

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();       
    }

    // Update is called once per frame
    void Update()
    {
        if (volume.profile.TryGet<DepthOfField>(out depthOfField))
        {
            //depthOfField.focalLength.value = 
            depthOfField.focalLength.Override(focalLength.Evaluate(Time));
            //volume.
        }
    }

    public void SetTime(float p)
    {
        Time = p;
    }
}
