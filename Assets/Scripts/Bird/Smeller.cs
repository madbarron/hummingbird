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
        float closest = float.PositiveInfinity;
        float distance;
        ITasty closestItem = null;

        // Remove any item which is no longer tasty
        foreach(ITasty dead in targets.FindAll(delegate (ITasty item) { return !item.IsTasty(); }))
        {
            targets.Remove(dead);
        }

        // Find closest item that is still tasty
        foreach (ITasty item in targets)
        {
            distance = (player.transform.position - item.GetPosition()).magnitude;
            if (distance < closest)
            {
                closest = distance;
                closestItem = item;
            }
        }

        // Deliver smell report to brain
        if (closestItem != null)
        {
            player.ClosestEdible = closestItem;
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
