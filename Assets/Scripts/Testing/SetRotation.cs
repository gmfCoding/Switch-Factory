using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetRotation : MonoBehaviour
{

    public Vector2Int rotation;
    // Update is called once per frame
    void Update()
    {
        TileEntityVisual.SetModelRotation(null, transform, rotation);
    }
}
