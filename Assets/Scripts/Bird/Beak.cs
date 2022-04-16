using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Beak : MonoBehaviour
{
    [SerializeField]
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Feeder feeder = collision.GetComponent<Feeder>();

        if (feeder && feeder.IsTasty())
        {
            player.BeginDrinking(feeder);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Feeder feeder = collision.GetComponent<Feeder>();

        if (feeder)
        {
            player.EndDrinking();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Eatable eatable = collision.GetComponent<Eatable>();

        if (eatable)
        {
            player.Eat();
            Destroy(eatable.gameObject);
        }

        Feeder feeder = collision.GetComponent<Feeder>();
        if (feeder && feeder.IsTasty())
        {
            player.FeederAvailable(feeder);
        }
    }
}
