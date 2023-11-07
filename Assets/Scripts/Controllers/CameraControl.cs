using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float mouseSensitivity = 3.0f;
    [SerializeField] private float zoomSpeed = 6.0f;
    [SerializeField] private float minZoomDistance = 2.0f;
    [SerializeField] private float maxZoomDistance = 15.0f;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private Vector2 rotationXMinMax = new Vector2(-0, 90);
    [SerializeField] private float distanceFromTarget = 5.0f;
    [SerializeField] private Vector2 rotationStartPos = new Vector2(-0, 90);
    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;
    private float rotationY = 0;
    private float rotationX = 0;
    private float targetDistanceFromTarget;
    private bool isCameraRotationEnabled = false;

    void Start()
    {
        targetDistanceFromTarget = distanceFromTarget;
        currentRotation = rotationStartPos;
        HandleRotation();
    }

    void Update()
    {
        HandleZoom();
         isCameraRotationEnabled = Input.GetAxis("Fire3") > 0;
        if (isCameraRotationEnabled)
            HandleRotation();
        transform.localEulerAngles = currentRotation;
        transform.position = target.position - transform.forward * distanceFromTarget; 
    }

    void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        targetDistanceFromTarget += scrollInput * zoomSpeed;
        targetDistanceFromTarget = Mathf.Clamp(targetDistanceFromTarget, minZoomDistance, maxZoomDistance);
        distanceFromTarget = Mathf.Lerp(distanceFromTarget, targetDistanceFromTarget, Time.deltaTime / smoothTime);
        distanceFromTarget = Mathf.Clamp(distanceFromTarget, minZoomDistance, maxZoomDistance);
        
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
    }
}