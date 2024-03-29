using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to create scenery to the left and right of the camera, and optionally remove it when it's too far away
/// </summary>
public class SceneryTiler : MonoBehaviour
{
    public Transform Camera;

    // Distance ahead and behind to build stuff
    public float bufferRight;
    public float bufferLeft;

    // Library of tile options. They are all assumed to be tileWidth units wide
    public List<GameObject> tilePrefabs;
    public float tileWidth;

    // Whether the objects should skip deletion
    public bool persist = false;

    // Instantiated game objects. These are in order, the first element being the leftmost in the scene.
    private List<GameObject> tiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Seed tiles array with a starting tile
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
            tile = Instantiate(getNewTile(tiles[0]), transform);
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
            tile = Instantiate(getNewTile(tiles[lastTile]), transform);
            tile.transform.position = new Vector3(rightMost + tileWidth, transform.position.y);
            rightMost = tile.transform.position.x;
            tiles.Add(tile);
        }
    }

    /// <summary>
    /// Get a new tile that is guaranteed to be different from the old tile, if more than one tile are in the pool
    /// </summary>
    /// <param name="oldTile"></param>
    /// <returns></returns>
    protected GameObject getNewTile(GameObject oldTile)
    {
        GameObject nextPrefab = randomPrefab;

        while (tilePrefabs.Count > 1 && (oldTile.name == nextPrefab.name || oldTile.name == nextPrefab.name + "(Clone)"))
        {
            nextPrefab = randomPrefab;
        }

        return nextPrefab;
    }

    /// <summary>
    /// A random element from the list of tile prefabs
    /// </summary>
    protected GameObject randomPrefab
    {
        get
        {
            int index = (int)(tilePrefabs.Count * Random.value);

            return tilePrefabs[index];
        }
    }
}
