using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RengeGames.HealthBars;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    [Header("Connections")]
    [SerializeField]
    private GameObject bird;

    [SerializeField]
    private GameObject neck;

    [SerializeField]
    private GameObject reflectionTransform;

    public Animator flapAnimator;
    public SpriteRenderer birdRenderer;
    public UltimateCircularHealthBar healthBar;
    public TextMeshProUGUI scoreText;

    [Header("Mobility")]
    public float maxPower;
    public float maxPowerRadius;
    public float pitchRate;
    public float massGainPerScore;

    [Header("Health")]
    public bool godMode;
    public float health = 1f;
    public float healthChipRate = 0.1f;
    public float healthBarMult = 0.333f;
    public float healthPowerDrain = 0.1f;
    public float drinkRate = 0.5f;

    [Header("Events")]
    public UnityEvent onGameOver;
    public UnityEvent onGameStart;    // Triggered when the game begins
    public GameEvent gameStartEvent;  // Triggered when the game begins

    private bool flapping = false;
    private bool dead = false;
    private int score = 0;
    private float prevRotation = 0.5f;

    private ITasty closestEdible;
    private bool gameInProgress = false;

    public ITasty ClosestEdible { set { closestEdible = value; } }

    private DifficultyManager difficulty;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        difficulty = FindObjectOfType<DifficultyManager>();

        // Sleep until the game begins
        rigidbody2D.sleepMode = RigidbodySleepMode2D.StartAsleep;
        rigidbody2D.Sleep();

        flapAnimator.Play("Pitch", 1, .5f);
        flapAnimator.Play("Flap Up", 0, 1);
        flapAnimator.speed = 0;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (godMode) health = 0.5f;

        if (dead || !gameInProgress)
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

        // Chip health
        health -= healthChipRate * Time.deltaTime;

        updateFacing();
        updateHealthBar();
    }

    protected void updateHealthBar()
    {
        healthBar.SetPercent(health * healthBarMult);
    }

    // Point beak/body in direction of nearest tasty
    protected void updateFacing()
    {
        // Find where we want the beak to point
        Vector3 target = rigidbody2D.velocity.x >= 0 ? Vector3.right : Vector3.left;

        if (closestEdible != null)
        {
            target = closestEdible.GetPosition() - transform.position;
        }

        // Find the angle that would point us to the target
        Quaternion targetQuaternion = Quaternion.LookRotation(Vector3.forward, target);
        float rotation = targetQuaternion.eulerAngles.z;

        // Push us towards the target at the speed of pitchRate
        float animationTime = Mathf.Abs(rotation - 180) / 180;
        float time = Mathf.MoveTowards(prevRotation, animationTime, pitchRate * Time.deltaTime);

        // Use animation to set us in the right rotation
        flapAnimator.Play("Pitch", 1, time);
        prevRotation = time;

        // Flip left/right
        Vector3 scale = reflectionTransform.transform.localScale;
        if (target.x < 0)
        {
            scale = new Vector3(Mathf.Abs(scale.x) * -1, scale.y, scale.z);
        }
        else
        {
            scale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        reflectionTransform.transform.localScale = scale;
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

    public void StartGame()
    {
        DifficultySettings settings = difficulty.Settings;

        // Apply settings
        godMode = difficulty.Settings.godMode;
        drinkRate = difficulty.Settings.drinkRate;
        healthPowerDrain = difficulty.Settings.healthPowerDrain;
        massGainPerScore = difficulty.Settings.massGainPerScore;

        // Begin game
        gameInProgress = true;
        rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rigidbody2D.WakeUp();
        flapAnimator.speed = 1;
        Time.timeScale = 1;
        updateHealthBar();
        healthBar.gameObject.SetActive(!godMode);

        onGameStart?.Invoke();
        gameStartEvent.Raise();
    }

    public void Eat()
    {
        if (dead)
        {
            return;
        }
        score++;
        scoreText.text = score.ToString();

        // Gains
        rigidbody2D.mass += massGainPerScore;

        // Avoid destroyed object reference with this little bit of spaghetti
        closestEdible = null;
    }

    protected void Die()
    {
        if (!dead)
        {
            dead = true;
            onGameOver?.Invoke();
            rigidbody2D.constraints = RigidbodyConstraints2D.None;
            healthBar.gameObject.SetActive(false);
            flapAnimator.speed = 0;
        }
    }
}
