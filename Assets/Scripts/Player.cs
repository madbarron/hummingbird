using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    //public float powerScale;
    public float maxPower;
    public float maxPowerRadius;
    public Animator flapAnimator;
    public SpriteRenderer birdRenderer;

    private bool flapping = false;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    private void Update()
    {
        // If mouse is clicked
        if (Input.GetMouseButton(0))
        {
            if (!flapping)
            {
                flapping = true;
                flapAnimator.SetTrigger("Down");

                // Find relative direction and distance
                Vector3 direction3 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                if (direction3.magnitude > maxPowerRadius)
                {
                    direction3 = direction3.normalized * maxPowerRadius;
                }

                direction3 *= maxPower / maxPowerRadius;


                rigidbody2D.AddForce(new Vector2(direction3.x, direction3.y), ForceMode2D.Impulse);
                Debug.Log(count++);
            }
        }
        else if (flapping)
        {
            flapping = false;
            flapAnimator.SetTrigger("Up");
        }

        // Point sprite in direction of travel
        bool goingLeft = rigidbody2D.velocity.x < 0;
        birdRenderer.flipX = goingLeft;
    }
}
