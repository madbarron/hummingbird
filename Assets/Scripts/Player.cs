using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using RengeGames.HealthBars;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    //public float powerScale;
    public float maxPower;
    public float maxPowerRadius;
    public Animator flapAnimator;
    public SpriteRenderer birdRenderer;
    public UltimateCircularHealthBar healthBar;

    public float health = 1f;
    public float healthChipRate = 0.1f;
    public float healthBarMult = 0.333f;
    public float healthPowerDrain = 0.1f;

    public float drinkRate = 0.5f;

    private bool flapping = false;

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

                health -= healthPowerDrain * (direction3.magnitude / maxPower);

                rigidbody2D.AddForce(new Vector2(direction3.x, direction3.y), ForceMode2D.Impulse);
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

        // Chip health
        health = Mathf.Clamp(health - healthChipRate * Time.deltaTime, 0, 1);

        healthBar.SetPercent(health * healthBarMult);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        Feeder feeder = collision.gameObject.GetComponent<Feeder>();

        if (feeder == null)
        {
            return;
        }

        float toDrink = Mathf.Clamp(1 - health, 0, drinkRate * Time.deltaTime);
        if (toDrink > 0)
        {
            float drank = feeder.DrainEnergy(toDrink);
            health += drank;
            Debug.Log(drank);
        }
    }
}
