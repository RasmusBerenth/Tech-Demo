using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //TODO (This week):
    //Set mouse to center of the screen.
    //Move camera with the mouse.
    //Rotate Player object with camera.
    //WASD or arrows to move.
    //Space to jump.
    //Hold shift to increase movementspeed.

    private PlayerControlls controlls;

    private Rigidbody rb;
    public Transform pt;

    private float mouseSensativety = 100f;
    private float xRotation = 0f;

    private float walkingSpeed = 5f;
    private float jumpHeight = 5f;

    private void Awake()
    {

        controlls = new PlayerControlls();

        controlls.Gameplay.Jump.performed += ctx => OnJump();
        controlls.Gameplay.Movement.performed += ctx => OnMovement(ctx.ReadValue<Vector2>());
        controlls.Gameplay.Movement.canceled += ctx => OnMovement(Vector2.zero);

        Cursor.lockState = CursorLockMode.Locked;


    }

    private void Update()
    {
        OnAim();
    }

    private void OnJump()
    {

    }

    private void OnMovement(Vector2 direction)
    {

    }

    private void OnAim()
    {
        Vector2 mouseDirection = controlls.Gameplay.Aim.ReadValue<Vector2>();

        float mouseX = mouseDirection.x * mouseSensativety * Time.deltaTime;
        float mouseY = mouseDirection.y * mouseSensativety * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        pt.Rotate(Vector3.up * mouseX);
    }

    private void OnEnable()
    {
        controlls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controlls.Gameplay.Disable();
    }
}
