using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    public Transform cylinder1;
    public Transform cylinder2;

    public float speed;
    public float time;
    public float duration;

    [SerializeField]
    private bool active;

    public bool Active { get => active; set => active = value; }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;
        time += Time.deltaTime;
        if (time >= duration)
        {
            time = 0;
            active = false;
        }
        cylinder1.localEulerAngles = new Vector3(time * speed, 0, 0);
        cylinder2.localEulerAngles = new Vector3(-time * speed, 0, 0);
    }
}
