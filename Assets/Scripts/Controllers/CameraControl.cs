using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 3.0f;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceFromTarget = 4.0f;
    [SerializeField] private float zoomSpeed = 6.0f;
    [SerializeField] private float minZoomDistance = 2.0f;
    [SerializeField] private float maxZoomDistance = 15.0f;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private Vector2 rotationXMinMax = new Vector2(-0, 90);
    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;
    private float rotationY;
    private float rotationX;
    private float targetDistanceFromTarget;
    private bool isCameraRotationEnabled = false;

    void Start()
    {
        HandleRotation();
    }

    void Update()
    {
        HandleZoom();
        isCameraRotationEnabled = Input.GetKey(KeyCode.LeftShift);
        if (isCameraRotationEnabled)
            HandleRotation();
    }

    void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        targetDistanceFromTarget += scrollInput * zoomSpeed;
        targetDistanceFromTarget = Mathf.Clamp(targetDistanceFromTarget, minZoomDistance, maxZoomDistance);
        distanceFromTarget = Mathf.Lerp(distanceFromTarget, targetDistanceFromTarget, Time.deltaTime / smoothTime);
        distanceFromTarget = Mathf.Clamp(distanceFromTarget, minZoomDistance, maxZoomDistance);
        transform.position = target.position - transform.forward * distanceFromTarget;
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;
        distanceFromTarget = Mathf.Clamp(distanceFromTarget, minZoomDistance, maxZoomDistance);
        rotationX = Mathf.Clamp(rotationX, rotationXMinMax.x, rotationXMinMax.y);
        Vector3 nextRotation = new Vector3(rotationX, rotationY);
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;
        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}