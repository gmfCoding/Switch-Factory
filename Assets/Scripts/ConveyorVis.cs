using System.Collections.Generic;
using UnityEngine;
using QueuedGizmos;

public class ConveyorVis : MonoBehaviour
{
    public static ConveyorVis instance;
    public List<Conveyor> conveyors = new List<Conveyor>();

    public float height = 0.38f;
    public void Awake()
    {
        instance = this;
    }

    Vector2 GetConveyorItemPath(Vector2Int previousDirection, Vector2Int currentTile, Vector2 currentDirection, float tFactor)
    {
        Vector2 prev = currentTile - previousDirection;
        Vector2 prevCentre = prev + Vector2.one / 2f;

        Vector2 C = currentTile + Vector2.one / 2f;
        Vector2 A = prevCentre + (Vector2)previousDirection / 2f;
        Vector2 B = C + currentDirection / 2f;

        Vector2 AC = Vector2.Lerp(A, C, tFactor);
        Vector2 CB = Vector2.Lerp(C, B, tFactor);
        return Vector2.Lerp(AC, CB, tFactor);
    }

    public void Update()
    {
        foreach (var conv in conveyors)
        {
            DrawQueue.Add(DrawArrow.Create(conv.pos.toVec3XZ().TileToWorld() + Vector3.up + Vector2Int.one.toVec3XZ() / 2f, (conv.pos + conv.Direction).toVec3XZ().TileToWorld() + Vector3.up + Vector2Int.one.toVec3XZ() / 2f, Color.green));
            foreach (var item in conv.queue)
            {
                if (!item.item.ExistInWorld())
                    item.item.LinkTransform(Instantiate(item.item.item.Model).transform);
                Vector2 pos = GetConveyorItemPath(item.from, conv.pos, conv.Direction, item.pos);
                item.item.transform.position = new Vector3(pos.x, height, pos.y).TileToWorld();
            }
        }
    }
}