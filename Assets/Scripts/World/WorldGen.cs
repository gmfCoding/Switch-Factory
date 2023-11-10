using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    public World world;
    public WorldMesh mesh;
    
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
    }

    public void GenerateResources()
    {
        List<ResourceNodeInfo> all = Game.instance.GetAllAssets<ResourceNodeInfo>();
        Random.InitState(0);

        // Ideally we do patchs then singles
        all.Sort((x, y) => -x.spawnInfo.type.CompareTo(y.spawnInfo.type));

        for (int i = 0; i < all.Count; i++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                for (int x = 0; x < World.Width; x++)
                {
                    if (Game.instance.world.GetTile(new Vector2Int(x ,y)).type == TileBase.Floor)
                    {
                        if (UnityEngine.Random.Range(0.0f, 100.0f) > all[i].spawnInfo.chance)
                            continue;
                        if (all[i].spawnInfo.type == ResourceGroup.Single)
                        {
                            if (Game.instance.world.IsNearGroupInstance(ResourceGroup.Patch, new Vector2Int(x, y), 5))
                                continue;
                            if (Game.instance.world.IsNearGroupInstance(ResourceGroup.Single, new Vector2Int(x, y), 2))
                                continue;
                        }
                        else if (Game.instance.world.IsNearGroupInstance(ResourceGroup.Patch, new Vector2Int(x, y), 15))
                            continue;
                         Game.instance.world.AddResourceNode(all[i], new Vector2Int(x, y), Vector2Int.right);
                    }
                }
            }
        }
    }
}
