using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPrefab : MonoBehaviour
{
    public List<GameObject> Prefabs;

    // Start is called before the first frame update
    void Start()
    {
        int index = (int)(Random.value * Prefabs.Count);
        GameObject obj = Instantiate(Prefabs[index], transform);
    }
}
