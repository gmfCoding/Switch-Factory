using Palmmedia.ReportGenerator.Core.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class World : MonoBehaviour
{
    public float tickRate = 30;
    public float time = 0;

    [Range(0f, 1000)]
    public int width = 20;
    [Range(0f, 1000)]
    public int height = 20;

    public static int Width => Game.instance.world.width;
    public static int Height => Game.instance.world.height;

    short[,] background;
    Tile[,] tiles;

    HashSet<TileEntity> entities = new HashSet<TileEntity>();
    HashSet<ITickable> tickables = new HashSet<ITickable>();

    HashSet<ItemStack> dropped = new HashSet<ItemStack>();

    public void Awake()
    {
        tiles = new Tile[height, width];
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                tiles[j, i] = new Tile();
                tiles[j, i].type = TileBase.Floor;
            }
        }
        background = new short[height, height];

        Serialise();
    }


    public void Update()
    {
        time += Time.deltaTime;
        if (time > 1.0f / tickRate) {
            time -= 1.0f / tickRate;
            foreach (var t in tickables)
                t.Tick();
        }
    }

    public TileEntity GetTileEntity(Vector2Int pos)
    {
        if (!InBounds(pos))
            return null;
        return tiles[pos.y, pos.x].entity;
    }

    public void ClearTile(Vector2Int pos)
    {
        TileEntity prev = tiles[pos.y, pos.x].entity;
        if (prev != null)
        {
            entities.Remove(prev);
            if (prev is ITickable tickable)
                tickables.Remove(tickable);
            tiles[pos.y, pos.x].entity = null; // Important for cyclical ClearTile's from within OnEntityDestroyed.
            prev.OnEntityDestroyed();
        }
        Tile tile = tiles[pos.y, pos.x];
        tile.entity = null;
        tile.tile = null;
        tile.resourceID = 0;
        tile.resources = 0;
        tiles[pos.y, pos.x] = tile;
    }

    public TileEntity SetTile(TileInfo info, Vector2Int pos)
    {
        if (!InBounds(pos))
            return null;
        ClearTile(pos);
        if (info == null)
            return null;
        Tile tile = info.Create();
        if (tile.entity != null)
        {
            tile.entity.pos = pos;
            entities.Add(tile.entity);
            if (tile.entity is ITickable)
                tickables.Add(tile.entity as ITickable);
            tile.entity.OnEntityCreate();
            
        }
        tiles[pos.y, pos.x] = tile;
        return tile.entity;
    }

    public static Vector3 TileToWorldSpace(Vector2Int tile)
    {
        return new Vector3(tile.x, 0, tile.y);
    }

    public bool InBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;
    }

    public Tile GetTile(Vector2Int pos)
    {
        if (!InBounds(pos))
            return Tile.Empty;
        return tiles[pos.y, pos.x];
    }

    public void Serialise()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }
    //public void Serialise()
    //{
    //    var stream = File.Open(Application.persistentDataPath, FileMode.OpenOrCreate);
    //    BinaryWriter wr = new BinaryWriter(stream);
    //    var types = Game.instance.GetAllAssets<TileInfo>();
    //    foreach (var item in types)
    //    {
    //        wr.Write(item.GetType());
            
    //    }
    //    wr.Write(this.width);
    //    wr.Write(this.height);
    //    for (int y = 0; y < height; y++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            tiles[y, x].Write(wr);
    //        }
    //    }
    //}

}
