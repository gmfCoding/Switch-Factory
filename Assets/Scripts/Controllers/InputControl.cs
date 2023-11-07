using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputControl : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private PlayerInput playerInput;
    private InputActions playerInputActions;
    public float moveSpeed = 1f;
    public float rotateSpeed = 1f;
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new InputActions();
        playerInputActions.Player.Enable();
        //playerInputActions.Player.Interact.performed += Interact;
    }

    private void Update()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        Vector3 dir = (Camera.main.transform.position - transform.position).normalized;
        Vector3 relativeRight = Vector3.Cross(dir, Vector3.up).normalized;
        Vector3 relativeForward = Vector3.Cross(relativeRight, Vector3.up).normalized;

        Vector3 movement = (relativeRight * inputVector.x + relativeForward * inputVector.y ) * moveSpeed;

        transform.Translate(movement * Time.deltaTime, Space.World);
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }

    }

    //public void Interact(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //        Debug.Log("Interacted" + context.phase);
    //}
}   
