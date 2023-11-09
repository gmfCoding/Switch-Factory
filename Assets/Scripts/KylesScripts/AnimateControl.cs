using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateControl : MonoBehaviour
{
    public Animator animator;
    private Vector3 previousPosition;

    public float movementState = 0.1f;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        // Calculate movement based on change in position
        Vector3 movement = transform.position - previousPosition;

        // Check if the object is moving
        if (movement.magnitude > movementState)
        {
            // Set the animation state to "Walking"
            animator.SetBool("Walking", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            // Set the animation state to "Idle"
            animator.SetBool("Walking", false);
            animator.SetBool("Idle", true);
        }

        // Update previous position
        previousPosition = transform.position;
    }
}