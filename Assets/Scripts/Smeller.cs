using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Smeller : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private List<ITasty> targets = new List<ITasty>();

    public void Update()
    {
        if (targets.Count > 0)
        {
            player.ClosestEdible = targets[0];
        }
        else
        {
            player.ClosestEdible = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ITasty tasty = collision.GetComponent<ITasty>();
        
        if (tasty != null)
        {
            targets.Add(tasty);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ITasty tasty = collision.GetComponent<ITasty>();

        if (tasty != null)
        {
            targets.Remove(tasty);
        }
    }
}
