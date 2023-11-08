using QueuedGizmos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.RendererUtils;

public class Builder : MonoBehaviour
{

    private static Plane xzPlane = new Plane(Vector3.up, Vector3.zero);

    [SerializeField]
    private List<string> placeables;

    public TileInfo selectedInfo;
    private int selected;
    [SerializeField]
    private int rotationIndex;

    public GameObject ghost;
    public Material ghostMaterial;

    public TMPro.TMP_Text text;

    public int RotationIndex { 
        get => rotationIndex % 4; 
        set {
            rotationIndex = value;
            if (rotationIndex < 0)
                rotationIndex = 4 - (rotationIndex % 4);
            else
                rotationIndex = rotationIndex % 4;
        }
    }

    public int Selected { get => selected; set { 
            selected = value;
            selected = (placeables.Count - (-selected % placeables.Count)) % placeables.Count;
        }
    }

    public void Start()
    {
        ghost = GameObject.CreatePrimitive(PrimitiveType.Cube);
        placeables = Game.instance.GetAllAssets<TileInfo>().Select(x => x.Name).ToList();
        SetGhostMesh(Selected);
    }

    void SetGhostMesh(int index)
    {
        Destroy(ghost);
        selectedInfo = Game.instance.GetAsset<TileInfo>(placeables[index % placeables.Count]);
        if (selectedInfo == null || selectedInfo.Model == null)
            ghost = GameObject.CreatePrimitive(PrimitiveType.Cube);
        else
        {
            text.text = selectedInfo.Name;
            ghost = Instantiate(selectedInfo.Model);
        }
        var renderers = ghost.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = ghostMaterial;
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                renderers[i].materials[j] = ghostMaterial;

            }
        }
    }

    public Tile GetTileUnderMouse(out bool hit, out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (xzPlane.Raycast(ray, out float dist))
        {
            point = ray.GetPoint(dist);
            hit = true;
            return Game.instance.world.GetTile(TileUtil.WorldspaceToTile(point));
        }
        hit = false;
        point = -Vector3.one;
        return Tile.Empty;
    }

    public void Update()
    {
        Tile tile = GetTileUnderMouse(out bool hit, out Vector3 pos);
        Vector2Int tilePos = TileUtil.WorldspaceToTile(pos);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selected++;
            SetGhostMesh(Selected);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (tile.entity != null)
                tile.entity.Direction = tile.entity.Direction;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (tile.IsEmpty())
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                    RotationIndex--;
                else
                    RotationIndex++;
            }
            else if (tile.entity is TileEntity e)
               e.Direction = TileUtil.IndexTileRotaiton(TileUtil.TileRotationIndex(e.Direction) + (Input.GetKey(KeyCode.LeftShift) ? -1 : 1));
        }
        if (Input.GetMouseButtonDown(0) && tile.entity == null)
        {
            TileInfo selectedTile = Game.instance.GetAsset<TileInfo>(placeables[Selected]);
            Game.instance.world.SetTile(selectedTile, tilePos);
            var entity = Game.instance.world.GetTileEntity(tilePos);
            if (entity != null)
                entity.Direction = TileUtil.IndexTileRotaiton(RotationIndex);
        }
        else if (Input.GetMouseButtonDown(1) && tile.entity != null)
        {
            Game.instance.world.SetTile(null, tilePos);
        }
        if (hit)
        {
            ghost.transform.position = TileUtil.TileToWorldspace(tilePos, TileUtil.TileTransform.Centric);
            ghost.transform.rotation = Quaternion.LookRotation(TileUtil.IndexTileRotaiton(RotationIndex).toVec3XZ(), Vector3.up);
            DrawQueue.Add(DrawSphere.Create(pos, 0.4f, Color.red));
            DrawQueue.Add(DrawArrow.Create(ghost.transform.position + Vector3.up * 1.5f, ghost.transform.position + Vector3.up * 1.5f + TileUtil.IndexTileRotaiton(RotationIndex).toVec3XZ(), Color.red));
        }
    }
}
