using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportRegion : MonoBehaviour
{
    public Queue<ItemStack> grabables = new Queue<ItemStack>();

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Grabable>() is Grabable grab)
        {
            grabables.Enqueue(grab.item);
            Destroy(grab.gameObject);
        }
    }
}
