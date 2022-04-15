using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RengeGames.HealthBars;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject bird;

    [SerializeField]
    private GameObject neck;

    private Rigidbody2D rigidbody2D;
    public float maxPower;
    public float maxPowerRadius;
    public Animator flapAnimator;
    public SpriteRenderer birdRenderer;
    public UltimateCircularHealthBar healthBar;

    public float health = 1f;
    public float healthChipRate = 0.1f;
    public float healthBarMult = 0.333f;
    public float healthPowerDrain = 0.1f;

    public bool godMode;

    public float drinkRate = 0.5f;
    public UnityEvent onGameOver;
    public TextMeshProUGUI scoreText;

    private bool flapping = false;
    private bool dead = false;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (godMode) health = 1;

        if (dead)
        {
            return;
        }

        Vector2 clickLocation = Vector2.zero;
        bool didClick = false;

        if (Input.GetMouseButton(0))
        {
            clickLocation = Input.mousePosition;
            didClick = true;
        }
        else if (Input.touchCount > 0)
        {
            clickLocation = Input.GetTouch(0).position;
            didClick = true;
        }
       
        // If mouse is clicked
        if (didClick)
        {
            if (!flapping)
            {
                flapping = true;
                flapAnimator.SetTrigger("Down");

                // Find relative direction and distance
                Vector3 direction3 = Camera.main.ScreenToWorldPoint(new Vector3(clickLocation.x, clickLocation.y)) - transform.position;

                // Scale input to unit circle, 1 magnitude is 100% power
                direction3 /= maxPowerRadius;

                // Clamp desired vector according to remaining health
                float maxAvailablePower = Mathf.Clamp(health / healthPowerDrain, 0, 1);
                direction3 = Vector3.ClampMagnitude(direction3, maxAvailablePower);

                // Pay for the flap
                health -= healthPowerDrain * direction3.magnitude;

                // Scale from input to physics
                direction3 *= maxPower;

                rigidbody2D.AddForce(new Vector2(direction3.x, direction3.y), ForceMode2D.Impulse);
            }
        }
        else if (flapping)
        {
            flapping = false;
            flapAnimator.SetTrigger("Up");
        }

        // Point sprite in direction of travel, kinda
        Vector3 travel = new Vector3(-rigidbody2D.velocity.x, Mathf.Abs(rigidbody2D.velocity.y) + 2);
        bird.transform.rotation = Quaternion.LookRotation(Vector3.forward, travel);

        // Flip left/right
        Vector3 scale = bird.transform.localScale;
        if (rigidbody2D.velocity.x < 0)
        {
            scale = new Vector3(Mathf.Abs(scale.x) * -1, scale.y, scale.z);
            neck.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.left);

        }
        else
        {
            scale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
            neck.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
        }
        bird.transform.localScale = scale;

        // Chip health
        health -= healthChipRate * Time.deltaTime;
        healthBar.SetPercent(health * healthBarMult);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        Feeder feeder = collision.gameObject.GetComponent<Feeder>();

        if (feeder == null)
        {
            return;
        }


    }

    // Don't die until you run into something
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (health <= 0 && !flapping)
        {
            Die();
        }
    }

    public void FeederAvailable(Feeder feeder)
    {
        float toDrink = Mathf.Clamp(1 - health, 0, drinkRate * Time.deltaTime);
        if (toDrink > 0)
        {
            float drank = feeder.DrainEnergy(toDrink);
            health += drank;
        }
    }

    public void Eat()
    {
        if (dead)
        {
            return;
        }
        score++;
        scoreText.text = score.ToString();
    }

    protected void Die()
    {
        if (!dead)
        {
            dead = true;
            onGameOver?.Invoke();
            rigidbody2D.constraints = RigidbodyConstraints2D.None;
            healthBar.gameObject.SetActive(false);
        }
    }
}
