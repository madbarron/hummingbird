using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryTiler : MonoBehaviour
{
    public Transform Camera;
    public float bufferRight;
    public float bufferLeft;

    public List<GameObject> tilePrefabs;
    public float tileWidth;

    public List<GameObject> tiles = new List<GameObject>();
    public bool persist = false;

    // Start is called before the first frame update
    void Start()
    {
        if (tiles.Count == 0)
        {
            GameObject tile = Instantiate(tilePrefabs[0], transform);
            tile.transform.position = transform.position;
            tiles.Add(tile);
        }

        Evaluate();
    }

    // Update is called once per frame
    void Update()
    {
        Evaluate();
    }

    protected void Evaluate()
    {
        float cameraPos = Camera.position.x;
        float leftMost;
        float rightMost;
        int lastTile;
        GameObject tile;

        // Check to see if we need fewer tiles left
        leftMost = tiles[0].transform.position.x;

        while (!persist && leftMost < cameraPos - bufferLeft - tileWidth)
        {
            Destroy(tiles[0].gameObject);
            tiles.RemoveAt(0);
            leftMost = tiles[0].transform.position.x;
        }

        // Check to see if we need more tiles left
        while (leftMost > cameraPos - bufferLeft)
        {
            tile = Instantiate(randomPrefab, transform);
            tile.transform.position = new Vector3(leftMost - tileWidth, transform.position.y);
            leftMost = tile.transform.position.x;
            tiles.Insert(0, tile);
        }

        // Check if we need fewer tiles right
        lastTile = tiles.Count - 1;
        rightMost = tiles[lastTile].transform.position.x;

        while (!persist && rightMost > cameraPos + bufferRight + tileWidth)
        {
            Destroy(tiles[lastTile]);
            tiles.RemoveAt(lastTile);
            lastTile--;
            rightMost = tiles[lastTile].transform.position.x;

        }

        // Check if we need more tiles right
        if (rightMost < cameraPos + bufferRight)
        {
            tile = Instantiate(randomPrefab, transform);
            tile.transform.position = new Vector3(rightMost + tileWidth, transform.position.y);
            rightMost = tile.transform.position.x;
            tiles.Add(tile);
        }
    }

    protected GameObject randomPrefab
    {
        get
        {
            int index = (int)(tilePrefabs.Count * Random.value);
            return tilePrefabs[index];
        }
    }
}
