using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Array to hold our arrays of GameObjects
    [SerializeField] private TileData[] tileData;

    // Define which array set our first tile will always lead from
    [SerializeField] private TileData firstTile;

    // Define the previous tile instantiated
    private TileData previousTile;

    // Point of origin from where our track will start spawning
    [SerializeField] private Vector3 spawnOrigin = new Vector3(0, 0, 0);

    // Point at which our next tile will spawn
    private Vector3 spawnPoint;

    // Preset number of tiles to spawn at start
    [SerializeField] private int tilesToSpawn = 10;

    // Start is called before the first frame update
    void Start()
    {
        // Tell our script what the type is of the first tile
        previousTile = firstTile;

        // Generate initial map
        for (int i = 0; i < tilesToSpawn; i++)
        {
            SpawnTile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debug key to manually spawn new tiles
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnTile();
        }
    }

    TileData NextTile()
    {
        // Empty list for valid tiles to spawn next
        List<TileData> validTiles = new List<TileData>();

        // Empty TileData object
        TileData nextTile;

        // Initiate variable that will tell us which data object to look at for our next tile choice
        TileData.Direction nextDirection = TileData.Direction.North;

        // Loop through each to match the exit direction of the previous tile to teh appropriate TileData list
        switch (previousTile.exitDirection)
        {
            case TileData.Direction.North:
                nextDirection = TileData.Direction.South;
                spawnPoint = spawnPoint + new Vector3(0, 0, previousTile.tileSize.y);
                break;
            case TileData.Direction.East:
                nextDirection = TileData.Direction.West;
                spawnPoint = spawnPoint + new Vector3(previousTile.tileSize.x, 0, 0);
                break;
            case TileData.Direction.South:
                nextDirection = TileData.Direction.North;
                spawnPoint = spawnPoint + new Vector3(0, 0, -previousTile.tileSize.y);
                break;
            case TileData.Direction.West:
                nextDirection = TileData.Direction.East;
                spawnPoint = spawnPoint + new Vector3(-previousTile.tileSize.x, 0, 0);
                break;
            default:
                break;
        }
        
        // Loop through all our data object arrays
        for (int i = 0; i < tileData.Length; i++)
        {
            // Add our array if available tiles to the list we can pool from if exit and entry directions match
            if (tileData[i].entryDirection == nextDirection)
            {
                validTiles.Add(tileData[i]);
            }
        }

        // Pick a random tile from our list to be the next tile
        nextTile = validTiles[Random.Range(0, validTiles.Count)];

        return nextTile;
    }

    void SpawnTile()
    {

        TileData tileToSpawn = NextTile();
        GameObject tileFromList = tileToSpawn.tiles[Random.Range(0, tileToSpawn.tiles.Length)];

        previousTile = tileToSpawn;

        Instantiate(tileFromList, spawnPoint + spawnOrigin, Quaternion.identity);
    }
}
