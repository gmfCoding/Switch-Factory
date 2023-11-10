using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    public World world;
    public TerrainData terrain;

    public WorldMesh mesh;
    
    private float[,] map;


    public float scale;
    public float depth = 0.5f;

    public void Start()
    {
        GenerateLakes();
        GenerateResources();
    }

    [ContextMenu("Regenerate")]
    public void GenerateLakes()
    {
        int width = terrain.heightmapResolution;
        int height = terrain.heightmapResolution;
        map = new float[width, height];
        for (int y = 0; y < World.Height; y++)
        {
            for (int x = 0; x < World.Width; x++)
            {
                float sample = Mathf.PerlinNoise(x * scale, y * scale);
                if (sample > depth)
                {
                    //QueuedGizmos.DrawQueue.Add(QueuedGizmos.DrawCube.Create(TileUtil.TileToWorldspace(new Vector2Int(x, y), TileUtil.TileTransform.Centric), Vector3.one * 1f, Color.blue), true);
                    Game.instance.world.SetTile(Tile.Lava, new Vector2Int(x, y));
                }
                else
                    Game.instance.world.SetTile(Tile.Floor, new Vector2Int(x, y));
            }
        }
        mesh.Generate();
        //for (int y = 0; y < height - 1; y += 2)
        //{
        //    for (int x = 0; x < width - 1; x += 2)
        //    {
        //        if (Game.instance.world.GetTile(new Vector2Int(x/2, y/2)).type == TileBase.Lava)
        //        {
        //            // to world space: (x or y) / (res - 1f) * 512 - 250)
        //            map[y, x] = 0;
        //        }
        //        else
        //        {
        //            map[y, x] = 1;
        //        }
        //        map[y, x + 1] = map[y, x];
        //        map[y + 1, x] = map[y, x];
        //        map[y + 1, x + 1] = map[y, x];
        //    }
        //}
        //terrain.SetHeights(0, 0, map);
    }

    public void GenerateResources()
    {
        List<ResourceInfo> all = Game.instance.GetAllAssets<ResourceInfo>();
        for (int i = 0; i < all.Count; i++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                for (int x = 0; x < World.Width; x++)
                {
                    if (Game.instance.world.GetTile(new Vector2Int(x ,y)).type == TileBase.Floor)
                    {
                        if (all[i].spawnInfo.type == ResourceSpawnType.Single)
                        {
                            if (UnityEngine.Random.Range(0.0f, 100.0f) <= all[i].spawnInfo.chance)
                                Game.instance.world.SetTileResource(all[i], new Vector2Int(x, y));
                        }
                        else
                        {
                            float sample = Mathf.PerlinNoise(x * scale * 2 + 1723 * i + all[i].spawnInfo.offsetX, y * scale * 2 + 1723 * i + all[i].spawnInfo.offsetY);
                            if (sample > (1f - all[i].spawnInfo.size / 1000))
                            {
                                Game.instance.world.SetTileResource(all[i], new Vector2Int(x, y));
                            }
                        }
                    }
                }
            }
        }
    }
}
