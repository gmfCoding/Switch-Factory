using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMesh : MonoBehaviour
{
	private Vector3[] vertices;
	private Mesh mesh;

	public World world;

    public void Generate()
	{
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Grid";
		vertices = new Vector3[(world.height + 1) * (world.width + 1)];
		Vector2[] uv = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4(0f, 1f, 0f, -1f);
		for (int i = 0, y = 0; y <= world.height; y++)
		{
			for (int x = 0; x <= world.width; x++, i++)
			{
				if (world.GetTile(new Vector2Int(x, y)).type == TileBase.Lava)
					vertices[i] = new Vector3(x, 0, y);
				else
					vertices[i] = new Vector3(x, 1, y);
				uv[i] = new Vector2((float)x / world.width, (float)y / world.height);
				tangents[i] = tangent;
			}
		}
		mesh.vertices = vertices;
		mesh.uv = uv;
		int[] triangles = new int[world.width * world.height * 6];
		for (int ti = 0, vi = 0, y = 0; y < world.height; y++, vi++)
		{
			for (int x = 0; x < world.width; x++, ti += 6, vi++)
			{
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + world.width + 1;
				triangles[ti + 5] = vi + world.width + 2;
			}
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
}
