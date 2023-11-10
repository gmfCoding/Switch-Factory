using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public Vector3 amount;

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += amount * Time.deltaTime;
    }
}
