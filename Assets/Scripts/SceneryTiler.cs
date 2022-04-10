using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryTiler : MonoBehaviour
{
    public Transform Camera;
    public float bufferRight;
    public float bufferLeft;

    public GameObject tilePrefab;
    public float tileWidth;

    private List<GameObject> tiles;

    // Start is called before the first frame update
    void Start()
    {
        GameObject tile = Instantiate(tilePrefab, transform);
        tile.transform.position = transform.position;
        //tiles.Add(tile);

        tiles = new List<GameObject>() { tile };
    }

    // Update is called once per frame
    void Update()
    {
        float cameraPos = Camera.position.x;
        float leftMost;
        float rightMost;
        int lastTile;
        GameObject tile;

        // Check to see if we need fewer tiles left
        leftMost = tiles[0].transform.position.x;

        while (leftMost < cameraPos - bufferLeft - tileWidth)
        {
            Destroy(tiles[0].gameObject);
            tiles.RemoveAt(0);
            leftMost = tiles[0].transform.position.x;
            Debug.Log("Removed tile left");
        }

        // Check to see if we need more tiles left
        while (leftMost > cameraPos - bufferLeft)
        {
            tile = Instantiate(tilePrefab, transform);
            tile.transform.position = new Vector3(leftMost - tileWidth, transform.position.y);
            leftMost = tile.transform.position.x;
            tiles.Insert(0, tile);
            Debug.Log("Added tile left");
        }

        // Check if we need fewer tiles right
        lastTile = tiles.Count - 1;
        rightMost = tiles[lastTile].transform.position.x;

        while (rightMost > cameraPos + bufferRight + tileWidth)
        {
            Destroy(tiles[lastTile]);
            tiles.RemoveAt(lastTile);
            lastTile--;
            rightMost = tiles[lastTile].transform.position.x;
            Debug.Log("Removed tile right");

        }

        // Check if we need more tiles right
        if (rightMost < cameraPos + bufferRight)
        {
            tile = Instantiate(tilePrefab, transform);
            tile.transform.position = new Vector3(rightMost + tileWidth, transform.position.y);
            rightMost = tile.transform.position.x;
            tiles.Add(tile);
            Debug.Log("Added tile right");
        }

    }
}
