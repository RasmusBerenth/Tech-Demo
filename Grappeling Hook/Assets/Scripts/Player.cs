using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerControlls controlls;
    private Rigidbody rb;
    private GrappelingHook hook;

    private bool isRunning;
    private bool isGrounded;
    private float xRotation = 0f;

    [Header("Transforms")]
    [SerializeField]
    private Transform groundCheackTransform;
    [SerializeField]
    private Transform cameraTransform;

    [Header("Movement veriables")]
    [SerializeField]
    private float mouseSensativety;
    [SerializeField]
    private float walkingSpeed;
    [SerializeField]
    private float runningSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float maxSpeed;

    [Header("GroundCheack veriables")]
    [SerializeField]
    private float groundRadious;
    [SerializeField]
    private LayerMask whatIsGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hook = GameObject.Find("hookBase").GetComponent<GrappelingHook>();

        //Sets up controll map.
        controlls = new PlayerControlls();

        controlls.Gameplay.Jump.performed += ctx => OnJump();
        controlls.Gameplay.Run.performed += ctx => OnRunning(ctx);
        controlls.Gameplay.Run.canceled += ctx => OnRunning(ctx);
    }

    private void Update()
    {
        //Cheacks whether the character is on the ground or not.
        isGrounded = Physics.CheckSphere(groundCheackTransform.position, groundRadious, whatIsGround);

        //Changing speed limit when grappeling.
        if (hook.isGrappeling)
        {
            maxSpeed = 15;
        }
        else
        {
            maxSpeed = 11;
        }

        //Makes sure that movement is possible.
        OnWalking();
    }
    private void FixedUpdate()
    {
        //Sets speed limit for player.
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
    private void LateUpdate()
    {
        //Lets the player look around.
        OnAim();
    }

    private void OnAim()
    {
        Vector2 mouseDirection = controlls.Gameplay.Aim.ReadValue<Vector2>();
        //mouseSensativety is set to 100 in the editor.
        //mouseSensativety needs to be changed from 100 to 8 when making build.(Not sure why...)
        float mouseX = mouseDirection.x * mouseSensativety * Time.deltaTime;
        float mouseY = mouseDirection.y * mouseSensativety * Time.deltaTime;

        //Turns the camera with the cursor when it moves up an down.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 60);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        cameraTransform.Rotate(Vector3.up * mouseX);

        //rotate player left and right
        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnWalking()
    {
        Vector3 moveDirection = controlls.Gameplay.Movement.ReadValue<Vector3>();

        //Makes shore that the player can't move while swinging.
        if (hook.isAttached)
        {
            controlls.Gameplay.Movement.Disable();
        }
        else
        {
            controlls.Gameplay.Movement.Enable();
        }

        //Alter movement speed depending on inputs.
        if (isRunning == true)
        {
            transform.Translate(moveDirection * runningSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            transform.Translate(moveDirection * walkingSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void OnJump()
    {
        //Makes the player jump when they are on the ground.
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    private void OnRunning(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
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
