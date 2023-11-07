using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QueuedGizmos;
using JetBrains.Annotations;
using Unity.VisualScripting;

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
            DrawQueue.Add(DrawArrow.Create(conv.pos.toVec3XZ() + Vector3.up + Vector2Int.one.toVec3XZ() / 2f, (conv.pos + conv.Direction).toVec3XZ() + Vector3.up + Vector2Int.one.toVec3XZ() / 2f, Color.green));
            foreach (var item in conv.queue)
            {
                if (!item.item.ExistInWorld())
                    item.item.LinkTransform(Instantiate(item.item.item.Model).transform);
                Vector2 pos = GetConveyorItemPath(item.prev_dir, conv.pos, conv.Direction, item.pos);
                item.item.transform.position = new Vector3(pos.x, height, pos.y);
            }
        }
    }
}

/*
public Vector2 MathfGetCircleOffset(Vector2Int prev_dir, Vector2Int new_dir, float frac)
{
    float re = 0;
    if (prev_dir.y == 1 && new_dir.x == -1) // UP -> LEFT
        re = frac / 4f;
    else if (prev_dir.y == -1 && new_dir.x == -1) // DOWN -> LEFT
        re = frac / -4f;
    else if (prev_dir.y == 1 && new_dir.x == 1) // UP -> RIGHT
        re = (2f - frac) / 4f;
    else if (prev_dir.y == -1 && new_dir.x == 1) // DOWN -> RIGHT
        re = (2f - frac) / -4f;
    else if (prev_dir.x == 1 && new_dir.y == -1) // RIGHT -> DOWN
        re = (1f - frac) / 4f;
    else if (prev_dir.x == -1 && new_dir.y == -1) // LEFT -> DOWN
        re = (3f - frac) / 4f;
    else if (prev_dir.x == 1 && new_dir.y == 1) // RIGHT -> UP
        re = (1f - frac) / 4f;
    else if (prev_dir.x == -1 && new_dir.y == 1) // LEFT -> UP
        re = (1f - frac) / -4f;
  //  print($"{prev_dir} to {new_dir}: {frac} is {re} ");
    Vector2 pos = new Vector2(Mathf.Cos(re * Mathf.PI * 2)/2f, Mathf.Sin(re * Mathf.PI * 2)/2f);
    return (pos);
}
/*
public Vector2 MathfGetAsterioidOffset(Vector2Int prev_dir, Vector2Int new_dir, float frac)
{
    Vector2 pos = new Vector2(0,0);
    if (new_dir.y == 1) // UP
    {
        if (prev_dir.x == 1) // RIGHT
        {
            pos.x = Mathf.Cos(-frac + 2 * Mathf.PI / 2.0f);
            pos.y = Mathf.Sin(-frac + 2 * Mathf.PI / 2.0f);
        }
        else if (prev_dir.x == -1) // LEFT
        {
            pos.x = Mathf.Cos(frac * Mathf.PI / 2.0f);
            pos.y = Mathf.Sin(frac * Mathf.PI / 2.0f);
        }
    }
    else if (new_dir.y == -1) // DOWN
    {
        if (prev_dir.x == 1) // RIGHT
        {
            pos.x = Mathf.Cos(frac + 3f * Mathf.PI / 2.0f);
            pos.y = Mathf.Sin(frac + 3f * Mathf.PI / 2.0f);
        }
        else if (prev_dir.x == -1) // LEFT
        {
            pos.x = Mathf.Cos(-frac + 4.0f * Mathf.PI / 2.0f);
            pos.y = Mathf.Sin(-frac + 4.0f * Mathf.PI / 2.0f);
        }
    }
    else if (new_dir.x == 1) // LEFT
    {
        if (prev_dir.y == 1) // UP
        {
            pos.x = Mathf.Cos(-frac + 3f * Mathf.PI / 2.0f); ///V   
            pos.y = Mathf.Sin(-frac + 3f * Mathf.PI / 2.0f); ///V
        }
        else if (prev_dir.y == -1) // DOWN
        {
            pos.x = Mathf.Cos(frac + 1f * Mathf.PI / 2.0f);
            pos.y = Mathf.Sin(frac + 1f * Mathf.PI / 2.0f);
        }
    }
    else if (new_dir.x == -1) // RIGHT
    {
        if (prev_dir.y == 1) // UP
        {
            pos.x = Mathf.Cos(frac + 3f * Mathf.PI / 2.0f);
            pos.y = Mathf.Sin(frac + 3f * Mathf.PI / 2.0f);
        }
        else if (prev_dir.y == -1) // DOWN
        {
            pos.x = Mathf.Cos(-frac + 4.0f * Mathf.PI / 2.0f);
            pos.y = Mathf.Sin(-frac + 4.0f * Mathf.PI / 2.0f);
        }
    }
    pos.x = pos.x * pos.x * pos.x / 2f;
    pos.y = pos.y * pos.y * pos.y / 2f;
    return (pos);
}
    */
/*
public void Start()
{
    Vector2Int[] points =
    {
        Vector2Int.left, Vector2Int.up,
        Vector2Int.up, Vector2Int.right,
        Vector2Int.right, Vector2Int.down,
        Vector2Int.down, Vector2Int.left,
        Vector2Int.up, Vector2Int.left,
        Vector2Int.right, Vector2Int.up,
        //Vector2Int.down, Vector2Int.right,
        //Vector2Int.left, Vector2Int.down,
    };
    Color[] colours =
    {
        Color.HSVToRGB(1/4f*0f,1,1),
        Color.HSVToRGB(1/4f*1f,1,1),
        Color.HSVToRGB(1/4f*2f,1,1),
        Color.HSVToRGB(1/4f*3f,1,1),
    };
    const float res = 1/8f;
    for (int i = 0; i < points.Length; i += 2)
    {
        for (float j = 0.0f; j <= 1.0f; j += res)
        {
            Vector2 from = MathfGetCircleOffset(points[i], points[i + 1], j);
            Vector2 to = MathfGetCircleOffset(points[i], points[i + 1], j + res);
            float re = (i / (float)points.Length) + ((j) / 4f);
            float re2 = (i / (float)points.Length) + ((j + res) / 4f);
            QueuedGizmos.DrawQueue.Add(QueuedGizmos.DrawLine.Create(new Vector3(from.x, (i / 8) / 5.0f, from.y), new Vector3(to.x, (i / 8) / 5.0f, to.y), Color.HSVToRGB(re, 1, 1)), true);
            from = new Vector2( Mathf.Cos(re * Mathf.PI * 2), Mathf.Sin(re * Mathf.PI * 2));
            to = new Vector2( Mathf.Cos(re2 * Mathf.PI * 2),  Mathf.Sin(re2 * Mathf.PI * 2));
            QueuedGizmos.DrawQueue.Add(QueuedGizmos.DrawLine.Create(new Vector3(from.x, (i / 8)/5.0f, from.y), new Vector3(to.x, (i / 8) / 5.0f, to.y), Color.HSVToRGB(re, 1, 1)), true);

        }
    }
}
*/


/*
 *                 //item.Key.transform.position = new Vector3(0.5f - conv.direction.x / 2f, 0f, 0.5f - conv.direction.y / 2f) + new Vector3(conv.pos.x + conv.direction.x * item.Value, 0, conv.pos.y + conv.direction.y * item.Value);
                /*item.item.transform.position = (new Vector3(conv.direction.y, 0, conv.direction.x) / 2.0f) + new Vector3(
                    conv.pos.x + conv.direction.x * item.pos,
                    0,
                    conv.pos.y + conv.direction.y * item.pos);
*                 // remap(q_value, w_from1, h_to1, t_from2, y_to2)    (q_value - w_from1) / (h_to1 - w_from1) * (y_to2 - t_from2) + t_from2;

               // simple(q, from, to) v(t-f)+f
               DrawQueue.Add(DrawSphere.Create(conv.pos.toVec3XZ() + (Vector3.right + Vector3.forward) / 2f, 0.25f, Color.red));
                DrawQueue.Add(DrawSphere.Create((conv.pos + conv.direction).toVec3XZ() + item.prev_dir.toVec3XZ() / 2f, 0.25f, Color.blue));
                DrawQueue.Add(DrawArrow.Create((conv.pos).toVec3XZ(), (conv.pos + conv.direction).toVec3XZ(), Color.green));

                //if (item.prev_dir - conv.direction != Vector2Int.zero)
                //{
                //    pos = MathfGetCircleOffset(item.prev_dir, conv.direction, item.pos);
                //}
                //else
                //{
                //    pos = new Vector2(conv.direction.y, conv.direction.x) / 2.0f + // Offset to center of each tile
                //        new Vector2(conv.pos.x + conv.direction.x * item.pos, conv.pos.y + conv.direction.y * item.pos); // calculate position
                //}
                //QueuedGizmos.DrawQueue.Add(QueuedGizmos.DrawSphere.Create((new Vector3(conv.direction.y, 0, conv.direction.x) / 2.0f) + new Vector3(
                //    conv.pos.x + conv.direction.x * item.pos,
                //    0,
                //    conv.pos.y + conv.direction.y * item.pos), 0.1f, Color.blue), false);
                //DrawQueue.Add(DrawSphere.Create((new Vector3(conv.direction.y, 0, conv.direction.x) / 2.0f) + new Vector3(
                //    conv.pos.x + conv.direction.x * item.pos,
                //    0,
                //    conv.pos.y + conv.direction.y * item.pos), 0.1f, Color.blue), false);
                //DrawQueue.Add(DrawArrow.Create(Vector3.zero, VectorUtil.V2ItoV3(conv.direction), Color.magenta), false);
*/