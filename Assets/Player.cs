using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int playerSpeed = 10;

    public PlayerInputActions playerInputActions;
    public PlayerInput playerInput;
    private Vector3 generateScale;
    private Vector3 newScale;

    // private void Update() {
    //     Move();
    // }

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        generateScale = transform.localScale;
        // playerInputActions.Player.GoUp.performed+= GoUp;
        playerInputActions.Player.Movement.performed += Movement_performed;
    }

    private void Update() {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        UpdateDirection(inputVector.x);
        
        transform.Translate(new Vector2(inputVector.x, inputVector.y) * playerSpeed * Time.deltaTime, Space.World);
        if(Keyboard.current.tKey.wasPressedThisFrame)
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
        if(Keyboard.current.yKey.wasPressedThisFrame)
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
    }

    private void UpdateDirection(float dir)
    {
        if(dir==0) return;
        newScale = transform.localScale;
        newScale.x = generateScale.x * Mathf.Sign(dir);
        transform.localScale = newScale;
    }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        // Debug.Log(context);
        Vector2 inputVector = context.ReadValue<Vector2>();
        transform.Translate(new Vector2(inputVector.x, inputVector.y) * playerSpeed * Time.deltaTime, Space.World);
    }

    public void GoUp(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if(context.performed)
        {
            Debug.Log("Hi" + context.phase);
        }
    }
    public void Submit(InputAction.CallbackContext context)
    {
        Debug.Log("Submit" + context);
    }

    private void Move()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            transform.Translate(Vector3.left * playerSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            transform.Translate(Vector3.up * playerSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(Vector3.right * playerSpeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            transform.Translate(Vector3.down * playerSpeed * Time.deltaTime, Space.World);
        }
    }
}
