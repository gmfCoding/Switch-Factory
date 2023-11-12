using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    Tile[,] tiles;

    HashSet<TileEntity> entities = new HashSet<TileEntity>();
    HashSet<ITickable> tickables = new HashSet<ITickable>();

    public List<ResourceNodeInstance> resourceNodes = new List<ResourceNodeInstance>();
    public Dictionary<ResourceGroup, ResourceNodeInstance> resourceGroups = new Dictionary<ResourceGroup, ResourceNodeInstance>();

    HashSet<ItemStack> dropped = new HashSet<ItemStack>();

    [Header("Resource spawn info")]
    public float scale;
    public float offsetX;
    public float offsetY;

    public void Awake()
    {
        InitialiseTiles(width, height);
    }

    public void Start()
    {
        GenerateResources();
    }

    public void Update()
    {
        time += Time.deltaTime;
        if (time > 1.0f / tickRate) {
            time -= 1.0f / tickRate;
            foreach (var t in tickables)
                t.Tick();
        }
        if (Input.GetKeyDown(KeyCode.P))
            Serialise();
        else if (Input.GetKeyDown(KeyCode.L))
            Deserialise();
        else if (Input.GetKeyDown(KeyCode.C))
            InitialiseTiles(width, height);
        else if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (GameObject g in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                int triangleCount = 0;
                foreach (MeshFilter m in g.GetComponentsInChildren<MeshFilter>())
                {
                    triangleCount += m.mesh.triangles.Length;
                }
                Debug.Log(g.name + " has " + triangleCount.ToString() + " triangles");
            }
        }
    }

    public ResourceNodeInstance IsNearGroupInstance(ResourceGroup group, Vector2Int pos, float max)
    {
        foreach (var item in resourceNodes)
        {
            if (Vector2Int.Distance(item.Position, pos) <= max)
                return item;
        }
        return null;
    }

    public ResourceNodeInstance AddResourceNode(ResourceNodeInfo info, Vector2Int pos, Vector2Int direction)
    {
        var obj = Instantiate(info.Model.gameObject).GetComponent<ResourceNodeInstance>();
        obj.Position = pos;
        obj.Direction = direction;
        resourceNodes.Add(obj);
        return obj;
    }

    public void GenerateResources()
    {
        //var res = Game.instance.GetAsset<ResourceInfo>("resource_iron_ore");
        //for (int y = 0; y < height; y++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        var s = Mathf.PerlinNoise((x * scale) + offsetX, (y * scale) + offsetY);
        //        if (s > 0.9)
        //        {
        //            SetTileResource(res, new Vector2Int(x, y));
        //        }
        //    }
        //}
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
        tiles[pos.y, pos.x] = tile;
    }
    public bool SetTile(Tile tile, Vector2Int pos)
    {
        if (!InBounds(pos))
            return false;
        ClearTile(pos);
        if (tile.entity != null)
        {
            tile.entity.pos = pos;
            entities.Add(tile.entity);
            if (tile.entity is ITickable)
                tickables.Add(tile.entity as ITickable);
            tile.entity.OnEntityCreate();
        }
        tiles[pos.y, pos.x] = tile;
        if (tile.entity != null && tile.entity.obj != null)
            tile.entity.obj.GetComponent<TileCallback>()?.OnCreated(this, pos);
        return true;
    }

    public TileEntity SetTileInfo(TileInfo info, Vector2Int pos)
    {
        if (!InBounds(pos))
            return null;
        ClearTile(pos);
        if (info == null)
            return null;
        Tile tile = info.Create();
        SetTile(tile, pos);
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
        var stream = File.Open(Path.Combine(Application.persistentDataPath, "world.dat"), FileMode.OpenOrCreate);
        BinaryWriter wr = new BinaryWriter(stream);
        wr.Write(this.width);
        wr.Write(this.height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tile tile = tiles[y, x];
                wr.Write(tile.tile == null);
                if (tile.tile == null)
                    continue;
                var hasEntity = tile.entity != null;
                var hasTile = tile.tile != null;
                wr.Write(hasEntity);
                wr.Write(hasTile);
                if (hasTile)
                { 
                    wr.Write(tile.tile.Name);
                }
                if (tile.entity != null)
                { 
                    wr.Write(tile.entity.Direction.x);
                    wr.Write(tile.entity.Direction.y);
                }
            }
        }
    }

    public void InitialiseTiles(int newWidth, int newHeight)
    {
        if (tiles != null)
        {
            for (int j = 0; j < tiles.GetLength(0); j++)
                for (int i = 0; i < tiles.GetLength(1); i++)
                    ClearTile(new Vector2Int(j, i));
        }
        width = newWidth;
        height = newHeight;
        tiles = new Tile[height, width];
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                tiles[j, i] = new Tile();
                tiles[j, i].type = TileBase.Floor;
            }
        }
    }

    public void Deserialise()
    {
        var stream = File.Open(Path.Combine(Application.persistentDataPath, "world.dat"), FileMode.OpenOrCreate);
        BinaryReader rd = new BinaryReader(stream);

        InitialiseTiles(tiles.GetLength(0), tiles.GetLength(1));
        this.width = rd.ReadInt32();
        this.height = rd.ReadInt32();
        tiles = new Tile[height, width];
        InitialiseTiles(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (rd.ReadBoolean())
                    continue;
                var pos = new Vector2Int(x, y);
                var hasEntity = rd.ReadBoolean();
                var hasTile = rd.ReadBoolean();
                if (hasTile)
                    SetTileInfo(Game.instance.GetAsset<TileInfo>(rd.ReadString()), pos);
                if (hasEntity)
                {
                    GetTileEntity(pos).Direction = new Vector2Int(rd.ReadInt32(), rd.ReadInt32());
                }
            }
        }
    }

    internal void SetTileInternal(Tile tile, Vector2Int pos)
    {
        if (InBounds(pos))
            tiles[pos.y, pos.x] = tile;
    }
}
