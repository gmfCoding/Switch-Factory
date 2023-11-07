using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DelayedGizmosRenderer : MonoBehaviour
{
    public bool draw = true;
#if UNITY_EDITOR
    public void Awake()
    {
        EditorApplication.playModeStateChanged += Clear;
    }

    public void OnDisable()
    {
        EditorApplication.playModeStateChanged -= Clear;
    }

    public void OnDestroy()
    {
        EditorApplication.playModeStateChanged -= Clear;
    }

    public void Update()
    {
       // Debug.Log("Drawing:" + QueuedGizmos.DrawQueue.GetAmount().ToString() + " Gizmos");
    }

    public void OnDrawGizmos()
    {
        if (draw)
            QueuedGizmos.DrawQueue.DrawAll();
    }

    void Clear(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
            Clear();
    }

    [ContextMenu("Clear Permament")]
    void Clear()
    {
        QueuedGizmos.DrawQueue.Clear();
    }
#endif
}