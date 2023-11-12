using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform follow;
    public Transform rotor;
    public float speed = 3.0f;

    [SerializeField] private float mouseSensitivity = 3.0f;

    private float rotationY = 0;
    private float rotationX = 0;
    private Vector3 currentRotation;
    [SerializeField] private Vector2 rotationXMinMax = new Vector2(-0, 90);
    [SerializeField] private Vector2 rotationStartPos = new Vector2(-0, 90);    public void Start()
    {
        rotationX = rotor.localEulerAngles.x;
    }
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

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, rotationXMinMax.x, rotationXMinMax.y);
            Vector3 nextRotation = new Vector3(rotationX, 0);
            rotor.localEulerAngles = nextRotation;
            follow.eulerAngles += new Vector3(0, mouseX, 0);
        }
        wish.Normalize();
        if (Game.instance.world.GetTile((follow.position + wish / 8.0f + Vector3.forward * 0.5f + Vector3.right * 0.5f).WorldToTile()).type == TileBase.Floor)
            follow.position += wish * speed * Time.deltaTime;
    }
}
