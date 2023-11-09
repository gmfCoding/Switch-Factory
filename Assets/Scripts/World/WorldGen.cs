using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    public World world;
    public TerrainData terrain;
    
    private float[,] map;


    public float scale;
    public float depth = 0.5f;

    public void Awake()
    {
        GenerateLakes();
    }

    [ContextMenu("Regenerate")]
    public void GenerateLakes()
    {
        int width = terrain.heightmapResolution;
        int height = terrain.heightmapResolution;
        map = new float[width, height];
        for (int y = 0; y < height - 1; y += 2)
        {
            for (int x = 0; x < width - 1; x += 2)
            {

                map[y, x] = Mathf.PerlinNoise(x * scale, y * scale);
                if (map[y, x] > depth)
                    map[y, x] = 0;
                else
                    map[y, x] = 1;
                map[y, x + 1] = map[y, x];
                map[y + 1, x] = map[y, x];
                map[y + 1, x + 1] = map[y, x];
            }
        }
        terrain.SetHeights(0, 0, map);
    }
}
