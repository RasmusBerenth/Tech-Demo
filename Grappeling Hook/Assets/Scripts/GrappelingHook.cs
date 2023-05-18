using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappelingHook : Rope
{
    private PlayerControlls controlls;
    private Rigidbody hookRigidbody;
    private SphereCollider hookCollider;

    private bool isAttached = false;
    private bool hit = false;

    private Modes grappelingHookMode;

    [Header("Grappelinghook Variables")]
    [SerializeField]
    private GameObject gunObject;

    public GameObject hookObject;
    public GameObject ropeStartPoint;

    [SerializeField]
    private float hookSpeed;
    [SerializeField]
    private float grappelingSpeed;

    private Vector3 localPositionStart;
    private Quaternion localRotationStart;

    private void Awake()
    {
        controlls = new PlayerControlls();

        hookRigidbody = GetComponent<Rigidbody>();
        hookCollider = GetComponent<SphereCollider>();

        controlls.Gameplay.Shoot.performed += ctx => OnShoot();
        controlls.Gameplay.Switch.performed += ctx => OnSwitchMode();
    }

    public new void Start()
    {
        base.Start();

        localPositionStart = transform.localPosition;
        localRotationStart = transform.localRotation;
    }

    private void Update()
    {
        if (hit)
        {
            StopAllCoroutines();
        }
    }

    private void OnShoot()
    {
        //If the hook isn't attached to a target then the player is able to shoot the hook.
        if (!isAttached)
        {
            transform.parent = null;

            hookRigidbody.AddRelativeForce(hookSpeed * Vector3.forward, ForceMode.Impulse);
            hookCollider.enabled = true;
            hookRigidbody.useGravity = true;

            //The hook attepts to come back after 3 seconds.
            StartCoroutine(Recall(3));

        }
        else
        {
            hit = false;
            StartCoroutine(Recall(1));
        }
    }

    //Switches between the two modes.
    private void OnSwitchMode()
    {
        if (grappelingHookMode == Modes.Grappeling)
        {
            grappelingHookMode = Modes.Swinging;
        }
        else if (grappelingHookMode == Modes.Swinging)
        {
            grappelingHookMode = Modes.Grappeling;
        }
    }

    IEnumerator Recall(float timer)
    {
        yield return new WaitForSeconds(timer);

        hit = false;

        //If the hook isn't attached return the hook to the gun.
        hookRigidbody.velocity = Vector3.zero;

        isAttached = false;
        hookCollider.enabled = false;
        hookRigidbody.useGravity = false;

        hookObject.transform.SetParent(gunObject.transform);

        hookObject.transform.localPosition = localPositionStart;
        hookObject.transform.localRotation = localRotationStart;


    }

    //What happens if you hit a target.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            hit = true;
            isAttached = true;
            hookRigidbody.velocity = Vector3.zero;
            hookObject.transform.position = other.transform.position;
            hookRigidbody.useGravity = false;

            if (grappelingHookMode == Modes.Grappeling)
            {

            }

        }


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
