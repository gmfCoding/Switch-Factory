using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assembler : MonoBehaviour
{
    public Transform gear1;
    public Transform gear2;
    public Transform gear3;
    public Transform gear4;


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
        gear1.Rotate(Vector3.right, time * speed);
        gear2.Rotate(Vector3.right, time * speed);
        gear3.Rotate(Vector3.right, time * speed);
        gear4.Rotate(Vector3.right, time * speed);
        /*gear1.localEulerAngles += new Vector3(time * speed, 0, 0);
        gear2.localEulerAngles += new Vector3(time * speed, 0, 0);
        gear3.localEulerAngles += new Vector3(time * speed, 0, 0);
        gear4.localEulerAngles += new Vector3(time * speed, 0, 0);*/
    }
}
