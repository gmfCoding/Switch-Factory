using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportToWin : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Grabable>() is Grabable grab)
        {
            Destroy(grab.gameObject);
            MainObjective.instance.Tokens++;
        }
    }
}
