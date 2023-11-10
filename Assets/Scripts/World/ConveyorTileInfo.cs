using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "convTile.asset", menuName = "Game/Tiles/ConveyorTile")]
public class ConveyorTileInfo : BuildableTileInfo
{
    [SerializeField]
    private Vector2Int direction;

    public Vector2Int Direction { get => direction; }
}
