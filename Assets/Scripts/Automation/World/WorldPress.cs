using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPress : MonoBehaviour
{
    public Transform press;

    public float yMin;
    public float yMax;
    public float current;
    public float speed;
    public bool reverse;
    public float time;

    [SerializeField]
    private bool active;

    public bool Active { get => active; set => active = value; }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            time = 0;
            current = yMax;
            return;
        }
        time += Time.deltaTime * speed;
        if (reverse)
            current = Mathf.Lerp(yMax, yMin, time);
        else
            current = Mathf.Lerp(yMin, yMax, time);
        if (time >= 1.0f)
        {
            if (!reverse)
                Active = false;
            reverse = !reverse;
            time = 0f;
        }
        press.localPosition = new Vector3(0, current, 0);
    }
}
