using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour, IItemContainer
{
    [Range(0f, 1000)]
    int width = 10;
    [Range(0f, 1000)]
    int height = 10;

    short[,] background;
    Tile[,] tiles;

    HashSet<TileEntity> entities = new HashSet<TileEntity>();
    HashSet<ITickable> tickables = new HashSet<ITickable>();

    HashSet<Item> dropped = new HashSet<Item>();

    public void Awake()
    {
        tiles = new Tile[height, width];
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                tiles[j, i] = new Tile();
                tiles[j, i].type = TileType.Floor;
            }
        }
        background = new short[height, height];
    }

    public float tickRate = 30;
    public float time = 0;
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
        return tiles[pos.y, pos.x].entity;
    }

    public T CreateTileEntity<T>(Vector2Int pos) where T: TileEntity
    {
        T entity = Activator.CreateInstance<T>();
        if (entity is ITickable tickable)
            tickables.Add(tickable);
        entity.pos = pos;
        tiles[pos.y, pos.x].entity = entity;
        return (entity);
    }

    public void SetTile(Tile tile, Vector2Int pos)
    {
        TileEntity prev = tiles[pos.y, pos.x].entity;
        if (prev != null)
            entities.Remove(prev);
        if (prev != null && prev is ITickable tickable)
            tickables.Remove(tickable);
        tiles[pos.x, pos.y] = tile;
    }

    public void Remove(Item item)
    {
        throw new NotImplementedException();
    }

    public void Add(Item item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }
}
