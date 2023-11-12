using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutToWork : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float speed = 1.0f;
    private bool isMovingToA = true;

    private void Update()
    {
        Vector3 targetPosition = GetCurrentDestination();
        targetPosition.y = transform.position.y; // Maintain the current y-coordinate

        targetPosition.y = 0;


        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
        {
            SwapDestinations();
        }

        // Rotate towards the target position
        RotateTowards(targetPosition);
    }

    private Vector3 GetCurrentDestination()
    {
        return isMovingToA ? pointA.position : pointB.position;
    }

    private void SwapDestinations()
    {
        isMovingToA = !isMovingToA;
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 directionToDestination = targetPosition - transform.position;
        directionToDestination.y = 0f;
        if (directionToDestination.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(directionToDestination, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * 360f);
        }
    }
}
