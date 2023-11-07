using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tile.asset", menuName = "Game/Tiles/Tile", order = 0)]
public class TileInfo : AssetInfo
{
    [SerializeField]
    private GameObject model;

    [SerializeField]
    private TileBase placement;

    [SerializeField]
    private bool directional;
    [SerializeField]
    private SerializableSystemType systemType;
    [SerializeField]
    private float height;

    public TileBase Placement { get => placement; }
    public GameObject Model { get => model; }
    public bool Directional { get => directional; }
    public float Height { get => height; }

    public virtual Tile Create()
    { 
        Tile tile = new Tile();
        tile.type = placement;
        tile.tile = this;
        if (!string.IsNullOrEmpty(systemType.Name))
        {
            tile.entity = Activator.CreateInstance(systemType.SystemType, new object[] { this }) as TileEntity;
            if (tile.entity == null)
                Debug.LogWarning($"Cannot create instance of {systemType.Name} for tile");
        }
        return (tile);
    }

    public static TileInfo GetTile(string name) => Game.instance.GetAsset<TileInfo>(name);
}
