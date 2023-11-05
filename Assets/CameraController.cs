using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform follow;
    public float speed = 3.0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 forward;
        Vector3 wish = Vector3.zero;
        forward = Vector3.Cross(Camera.main.transform.right, Vector3.up);
        if (Input.GetKey(KeyCode.A))
            wish -= Camera.main.transform.right;
        if (Input.GetKey(KeyCode.D))
            wish += Camera.main.transform.right;
        if (Input.GetKey(KeyCode.W))
            wish += forward;
        if (Input.GetKey(KeyCode.S))
            wish -= forward;

        if (Input.GetMouseButtonDown(2))
        {
            
        }
        wish.Normalize();
        follow.position += wish * speed * Time.deltaTime;
    }
}
