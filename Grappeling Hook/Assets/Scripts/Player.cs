using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerControlls controlls;
    private Rigidbody rb;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private bool isRunning;
    private bool isGrounded;

    [Header("Transforms")]
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform groundCheackTransform;

    [Header("Movement veriables")]
    [SerializeField]
    private float mouseSensativety;
    [SerializeField]
    private float walkingSpeed;
    [SerializeField]
    private float runningSpeed;
    [SerializeField]
    private float jumpHeight;

    [Header("GroundCheack veriables")]
    [SerializeField]
    private float groundRadious;
    [SerializeField]
    private LayerMask whatIsGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //Sets up controll map.
        controlls = new PlayerControlls();

        controlls.Gameplay.Jump.performed += ctx => OnJump();
        controlls.Gameplay.Run.performed += ctx => OnRunning(ctx);
        controlls.Gameplay.Run.canceled += ctx => OnRunning(ctx);

        //Sets the cursor to the center of the screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {


        //Cheacks whether the character is on the ground or not.
        isGrounded = Physics.CheckSphere(groundCheackTransform.position, groundRadious, whatIsGround);

        //Makes sure that movement and aiming goes smoothly fram to fram.
        OnAim();
        OnWalking();
    }

    private void OnJump()
    {
        //Makes the player jump when they are on the ground.
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    private void OnWalking()
    {
        Vector3 moveDirection = controlls.Gameplay.Movement.ReadValue<Vector3>();

        if (isRunning == true)
        {
            transform.Translate(moveDirection * runningSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            transform.Translate(moveDirection * walkingSpeed * Time.deltaTime, Space.Self);
        }
    }

    //While holding "run button" (in this case SHIFT) enable running.
    private void OnRunning(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    private void OnAim()
    {
        Vector2 mouseDirection = controlls.Gameplay.Aim.ReadValue<Vector2>();
        float mouseX = mouseDirection.x * mouseSensativety * Time.deltaTime;
        float mouseY = mouseDirection.y * mouseSensativety * Time.deltaTime;

        //Turns the camera with the cursor when it moves up an down.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 60);

        cameraTransform.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        cameraTransform.Rotate(Vector3.up * mouseX);

        //Turns the player with the cursor when it moves left or right.
        yRotation -= mouseX;
        playerTransform.transform.localRotation = Quaternion.Euler(0, -yRotation, 0);
        playerTransform.Rotate(Vector3.up * mouseX);
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
