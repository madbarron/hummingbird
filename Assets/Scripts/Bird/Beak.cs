using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component responsible for consuming tasty items
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Beak : MonoBehaviour
{
    [SerializeField]
    private Player player;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
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
        // Always eat eatables
        Eatable eatable = collision.GetComponent<Eatable>();
        if (eatable)
        {
            player.Eat();
            Destroy(eatable.gameObject);
        }

        // Player gets to decide what to do with feeders
        Feeder feeder = collision.GetComponent<Feeder>();
        if (feeder && feeder.IsTasty())
        {
            player.FeederAvailable(feeder);
        }
    }
}
