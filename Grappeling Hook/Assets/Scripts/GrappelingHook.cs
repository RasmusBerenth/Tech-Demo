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


    private void Awake()
    {
        controlls = new PlayerControlls();
        hookRigidbody = GetComponent<Rigidbody>();
        hookCollider = GetComponent<SphereCollider>();

        controlls.Gameplay.Shoot.performed += ctx => OnShoot();
        controlls.Gameplay.Switch.performed += ctx => OnSwitchMode();
    }

    private void OnShoot()
    {
        //If the hook isn't attached to a target then the player is able to shoot the hook.
        if (!isAttached)
        {
            transform.parent = null;

            Vector3 shootDirection = gunObject.transform.localPosition;

            hookRigidbody.AddForce(Vector3.forward.normalized * hookSpeed * Time.deltaTime, ForceMode.Impulse);
            hookCollider.enabled = true;
            hookRigidbody.useGravity = true;

            //The hook attepts to come back after 5...
            StartCoroutine(Recall(5));
        }
        else
        {
            //... or 1 seconds.
            Recall(1);
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

        //If the hook isn't attached return the hook to the gun.
        if (!isAttached)
        {
            hookCollider.enabled = false;
            hookRigidbody.useGravity = false;
            transform.position = new Vector3(0, 1, 0);
            hookRigidbody.velocity = Vector3.zero;
        }
    }

    private void Grapple()
    {

    }

    //What happens if you hit a target.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            isAttached = true;
        }

        if (grappelingHookMode == Modes.Grappeling)
        {
            Grapple();
        }
        else if (grappelingHookMode == Modes.Swinging)
        {

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
