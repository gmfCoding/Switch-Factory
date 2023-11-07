using QueuedGizmos;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TileEntityVisual
{
    private static string[] placeables = new string[]
    {
        "conv_straight",
        "conv_curved",
        "conv_tsection",
        "conv_intersection"
    };

    struct NeighbourVariation
    {
        public Vector2Int prev;
        public Vector2Int dir;
        public string model;

        public NeighbourVariation(Vector2Int prev, Vector2Int dir, string model)
        {
            this.prev = prev;
            this.dir = dir;
            this.model = model;
        }
    }

    private static NeighbourVariation[] variations = new NeighbourVariation[]
    {
        /*
        new NeighbourVariation(Vector2Int.down, Vector2Int.down, "conv_straight"),
        new NeighbourVariation(Vector2Int.left, Vector2Int.left, "conv_straight"),
        new NeighbourVariation(Vector2Int.right, Vector2Int.right, "conv_straight"),
        new NeighbourVariation(Vector2Int.up, Vector2Int.up, "conv_straight"),
        */
        new NeighbourVariation(Vector2Int.down, Vector2Int.left, "conv_anticurved"),
        new NeighbourVariation(Vector2Int.down, Vector2Int.right, "conv_curved"),
        new NeighbourVariation(Vector2Int.up, Vector2Int.right, "conv_anticurved"),
        new NeighbourVariation(Vector2Int.up, Vector2Int.left, "conv_curved"),

        new NeighbourVariation(Vector2Int.left,Vector2Int.down, "conv_curved"),
        new NeighbourVariation(Vector2Int.right, Vector2Int.down, "conv_anticurved"),
        new NeighbourVariation(Vector2Int.right, Vector2Int.up, "conv_curved"),
        new NeighbourVariation(Vector2Int.left, Vector2Int.up, "conv_anticurved"),
    };

    public static void RegenerateModel(TileEntity entity, Transform visual, bool prop = false)
    {
        if (entity == null)
            return;
        if (entity is Conveyor conv)
            conv.obj = RegenerateConveyorModel(conv.pos, conv.Direction, conv.obj, prop);
        else
        {
            if (visual == null)
                return;
            SetModelRotation(entity.Info, visual, entity.Direction);
        }
    }

    public static void SetModelPostion(TileInfo info, Transform obj, Vector2Int position)
    {
        obj.transform.position = TileUtil.TileToWorldspace(position, TileUtil.TileTransform.Centric);
        obj.transform.position += Vector3.up * info.Height;
    }

    public static void SetModelRotation(TileInfo info, Transform obj, Vector2Int rotation)
    {
        obj.rotation = Quaternion.LookRotation(rotation.toVec3XZ());
    }

    public static GameObject RegenerateConveyorModel(Vector2Int pos, Vector2Int dir, GameObject target, bool prop = false)
    {
        TileEntity eAbove = Game.instance.world.GetTileEntity(pos + Vector2Int.up);
        TileEntity eBelow = Game.instance.world.GetTileEntity(pos + Vector2Int.down);
        TileEntity eRight = Game.instance.world.GetTileEntity(pos + Vector2Int.right);
        TileEntity eLeft = Game.instance.world.GetTileEntity(pos + Vector2Int.left);

        Conveyor cAbove = eAbove as Conveyor;
        Conveyor cBelow = eBelow as Conveyor;
        Conveyor cLeft = eLeft as Conveyor;
        Conveyor cRight = eRight as Conveyor;

        if (prop)
        {
            if (cAbove != null)
                RegenerateConveyorModel(pos + Vector2Int.up, cAbove.Direction, cAbove.obj, false);
            if (cBelow != null)
                RegenerateConveyorModel(pos + Vector2Int.down, cBelow.Direction, cBelow.obj, false);
            if (cLeft != null)
                RegenerateConveyorModel(pos + Vector2Int.left, cLeft.Direction, cLeft.obj, false);
            if (cRight != null)
                RegenerateConveyorModel(pos + Vector2Int.right, cRight.Direction, cRight.obj, false);
        }

        // How many conveyors are pointing into this one?
        HashSet<Vector2Int> inputDirections = new HashSet<Vector2Int>();
        for (int i = 0; i < 4; i++)
        {
            Vector2Int ndir = TileUtil.IndexTileRotaiton(i);
            Conveyor nconv = Game.instance.world.GetTileEntity(pos + ndir) as Conveyor;
            if (nconv != null && (nconv.Direction + nconv.pos) == pos)
                inputDirections.Add(nconv.Direction);
        }
        int connected = inputDirections.Count;
        /*if (connected == 1 && Vector2.Dot(avail.Direction, dir) == 0)
            placeable = placeables[1];
        */
        string placeable = "conv_straight";
        if (connected == 1)
        {
            int i = 0;
            foreach (var item in variations)
            {
                if (item.prev == inputDirections.First() && item.dir == dir)
                {
                    DrawQueue.Add(DrawSphere.Create(pos.toVec3XZ() + Vector3.up * i, 0.15f, Color.red), true);
                    placeable = item.model;
                    break;
                }
                i++;
            }
        }
        else
        { 
            placeable = placeables[Mathf.Max(0, Mathf.Min(placeables.Length - 1, connected))];
   
        }
        var tile = Game.instance.GetAsset<ConveyorTileInfo>(placeable);
        if (target == null)
            target = GameObject.Instantiate(tile.Model);
        else
            target.GetComponent<MeshFilter>().mesh = tile.Model.GetComponent<MeshFilter>().sharedMesh;

        // Neighbour aware adjusted orientation 
    /*    int index = TileUtil.TileRotationIndex(dir);
        if (connected == 1 && Vector3.Cross(avail.Direction.toVec3XZ(), dir.toVec3XZ()).y < 0)
            index -= 1;*/
        SetModelPostion(tile, target.transform, pos);
        SetModelRotation(tile, target.transform, dir);
        return target;
    }
}
