using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //TODO (This week):
    //Hold shift to increase movementspeed.

    private PlayerControlls controlls;
    private new Rigidbody rigidbody;

    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform playerTransform;

    private float mouseSensativety = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    [SerializeField]
    private float walkingSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float runningSpeed;

    private void Awake()
    {
        controlls = new PlayerControlls();
        rigidbody = GetComponent<Rigidbody>();

        controlls.Gameplay.Jump.performed += ctx => OnJump();
        controlls.Gameplay.Run.performed += ctx => OnRunning(ctx);
        //controlls.Gameplay.Movement.performed += ctx => OnWalking(ctx.ReadValue<Vector3>());
        //controlls.Gameplay.Movement.canceled += ctx => OnWalking(Vector3.zero);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        OnAim();
        OnWalking();
    }

    private void OnJump()
    {
        rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }

    private void OnWalking()
    {
        //Vector3 moveDirection = controlls.Gameplay.Movement.ReadValue<Vector3>();
        //moveDirection.Normalize();
        transform.Translate(controlls.Gameplay.Movement.ReadValue<Vector3>() * walkingSpeed * Time.deltaTime, Space.Self);
    }

    private void OnRunning(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() == false)
        {
            walkingSpeed = runningSpeed;
        }
    }

    private void OnAim()
    {
        Vector2 mouseDirection = controlls.Gameplay.Aim.ReadValue<Vector2>();

        float mouseX = mouseDirection.x * mouseSensativety * Time.deltaTime;
        float mouseY = mouseDirection.y * mouseSensativety * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 60);

        cameraTransform.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        cameraTransform.Rotate(Vector3.up * mouseX);

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
