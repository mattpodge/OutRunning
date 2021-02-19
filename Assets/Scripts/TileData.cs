using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tile Data")]
public class TileData : ScriptableObject
{
    // Direction presets
    public enum Direction
    {
        North,
        East,
        South,
        West
    };

    // Set our tile size
    public Vector2 tileSize = new Vector2(1000.0f, 1000.0f);

    // Empty array to hold our GameObjects
    public GameObject[] tiles;

    // Direction of entry to tile
    public Direction entryDirection;

    // Direction of exit from tile
    public Direction exitDirection;
}
