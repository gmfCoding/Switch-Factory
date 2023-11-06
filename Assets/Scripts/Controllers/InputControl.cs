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
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new InputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact;
    }

    private void Update()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y) * moveSpeed;
        transform.Translate(movement * Time.deltaTime);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
            Debug.Log("Interacted" + context.phase);
    }
}
