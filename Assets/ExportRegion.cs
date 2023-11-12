using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportRegion : MonoBehaviour
{
    public GameObject worldItemPrefab;
    public Transform spawnPosition;

    public void AddItem(ItemStack item)
    {
        var obj = Instantiate(worldItemPrefab);
        var grabbable = obj.GetComponent<Grabable>();
        var rb = obj.GetComponent<Rigidbody>();
        grabbable.item = item;
        rb.position = spawnPosition.position;
        rb.AddForce(transform.forward + transform.up * 5, ForceMode.Impulse);
    }
}
