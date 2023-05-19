using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappelingHook : MonoBehaviour
{
    private PlayerControlls controlls;
    private Rigidbody hookRigidbody;
    private SphereCollider hookCollider;

    private bool isAttached = false;

    private Modes grappelingHookMode;

    [Header("Grappelinghook Variables")]
    [SerializeField]
    private GameObject gunObject;

    [SerializeField]
    private GameObject _hookObject;
    public GameObject hookObject => _hookObject;

    public GameObject ropeStartPoint;

    [SerializeField]
    private float hookSpeed;
    [SerializeField]
    private float grappelingSpeed;

    private Vector3 localPositionStart;
    private Quaternion localRotationStart;

    public GrappelingHook(GameObject hookObject, GameObject ropeStartPoint)
    {
        hookObject = this.hookObject;
        ropeStartPoint = this.ropeStartPoint;
    }

    private void Awake()
    {
        controlls = new PlayerControlls();

        hookRigidbody = GetComponent<Rigidbody>();
        hookCollider = GetComponent<SphereCollider>();

        controlls.Gameplay.Shoot.performed += ctx => OnShoot();
        controlls.Gameplay.Switch.performed += ctx => OnSwitchMode();
    }

    public void Start()
    {
        localPositionStart = transform.localPosition;
        localRotationStart = transform.localRotation;
    }

    private void Update()
    {
        if (isAttached)
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
            isAttached = false;
            StartCoroutine(Recall(1));
        }
    }

    //Switches between the two modes.
    private void OnSwitchMode()
    {
        if (grappelingHookMode == Modes.Swinging)
        {
            grappelingHookMode = Modes.Grappeling;
            Debug.Log(grappelingHookMode);
        }

        if (grappelingHookMode == Modes.Grappeling)
        {
            grappelingHookMode = Modes.Swinging;
            Debug.Log(grappelingHookMode);
        }

    }

    IEnumerator Recall(float timer)
    {
        yield return new WaitForSeconds(timer);

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
