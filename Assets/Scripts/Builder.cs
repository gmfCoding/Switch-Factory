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

    bool active;
    public TileInfo selectedInfo;
    private int selected;
    [SerializeField]
    private int rotationIndex;

    public GameObject ghost;
    public Material ghostMaterial;

    public TMPro.TMP_Text buildItem;

    public TMPro.TMP_Text tileDetails;

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

    public float RightClickTime
    {
        get => rightClickTime;
        set
        {
            rightClickTime = value;
            //rightClickVis.sizeDelta = new Vector2(5, (rightClickDuration - rightClickTime) * 100);
            //rightClickVis.sizeDelta = Vector2.right * 5 + Vector2.up * (rightClickTime / rightClickDuration) * 30;
        }
    }

    public float rightClickDuration = 0.2f;
    public RectTransform rightClickVis;
    private float rightClickTime = 0;
    public Vector2Int removeTile;


    public void Start()
    {
        ghost = GameObject.CreatePrimitive(PrimitiveType.Cube);
        placeables = Game.instance.GetAllAssets<TileInfo>().Where(x => x is BuildableTileInfo b && b.buildList).Select(x => x.Name).ToList();
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
            buildItem.text = selectedInfo.Name;
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
        tileDetails.text = tile.ToString();
        tileDetails.text += tilePos.ToString();
        if (Input.GetKeyDown(KeyCode.B))
            active = !active;
        if (!active)
            return;
        bool inverse = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inverse)
                Selected--;
            else
                Selected++;
            SetGhostMesh(Selected);
        }
        // Tile Deletion Change
        // We moved to a different tile reset the previous tile scale (deletion factor)
        if (tilePos != removeTile)
        {
            if (Game.instance.world.GetTileEntity(removeTile) is TileEntity e && e.obj != null)
                e.obj.transform.localScale = Vector3.one;
            RightClickTime = 0.0f;
        }
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    if (tile.entity != null)
        //        tile.entity.Direction = tile.entity.Direction;
        //}
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (tile.IsEmpty())
            {
                if (inverse)
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
            Game.instance.world.SetTileInfo(selectedTile, tilePos);
            var entity = Game.instance.world.GetTileEntity(tilePos);
            if (entity != null)
                entity.Direction = TileUtil.IndexTileRotaiton(RotationIndex);
        }
        else
        {
            // Tile deletion, hold over a tile for 0.2 (default seconds) to delete.
            // When the user moves on to the next tile before this one is deleted it will  start deleting that tile from T = 0. SEE Tile Deletion Change
            if (Input.GetMouseButton(1) && tile.entity != null && Game.instance.world.InBounds(tilePos))
            {
                removeTile = tilePos;
                RightClickTime += Time.deltaTime;
                // The longer we hold for the smaller the tile will be (once it reach size zero it will be deleted)
                tile.entity.obj.transform.localScale = Vector3.one * (1f - (RightClickTime / rightClickDuration));
                if (RightClickTime > rightClickDuration)
                {
                    Game.instance.world.SetTileInfo(null, removeTile);
                    removeTile = -Vector2Int.one;
                }
            }
            else
            {
                RightClickTime = 0.0f;
            }
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
