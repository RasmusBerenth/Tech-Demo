using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappelingHook : MonoBehaviour
{
    private PlayerControlls controlls;
    private Rigidbody hookRigidbody;
    private SphereCollider hookCollider;

    public bool isAttached = false;
    public bool isGrappeling = false;

    public Modes grappelingHookMode;

    private Vector3 localPositionStart;
    private Quaternion localRotationStart;
    private Vector3 localScaleStart;

    private SpringJoint joint;
    private Rope rope;

    [Header("Grappelinghook Variables")]
    [SerializeField]
    private GameObject gunObject;

    public GameObject hookObject;
    public GameObject ropeStartPoint;

    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private float hookSpeed;

    [Header("Joint Variables")]
    [SerializeField]
    float jointSpring;
    [SerializeField]
    float jointDamper;
    [SerializeField]
    float jointMassScale;


    private void Awake()
    {
        controlls = new PlayerControlls();

        hookRigidbody = GetComponent<Rigidbody>();
        hookCollider = GetComponent<SphereCollider>();
        rope = GetComponent<Rope>();

        controlls.Gameplay.Shoot.performed += ctx => OnShoot();
        controlls.Gameplay.Switch.performed += ctx => OnSwitchMode();

        grappelingHookMode = Modes.Swinging;
    }

    public void Start()
    {
        localRotationStart = transform.localRotation;
        localPositionStart = transform.localPosition;
        localScaleStart = transform.localScale;
    }

    private void Update()
    {
        if (isAttached) { StopAllCoroutines(); }
    }

    //Controlls grappeling effect.
    private void FixedUpdate()
    {
        if (isGrappeling)
        {
            //Moves player towards the hook.
            Vector3.MoveTowards(playerObject.transform.position, hookObject.transform.position, 1);
        }
    }

    private void OnShoot()
    {
        //If the hook isn't attached to a target then the player is able to shoot the hook.
        if (!isAttached)
        {
            transform.parent = null;
            hookRigidbody.isKinematic = false;
            hookRigidbody.AddRelativeForce(hookSpeed * Vector3.forward, ForceMode.Impulse);
            hookCollider.enabled = true;
            hookRigidbody.useGravity = true;

            //The hook attepts to come back after an assigned time.
            StartCoroutine(Recall(1f));
        }
        else
        {
            //Calls back the hook when allready attached.
            isAttached = false;
            StartCoroutine(Recall(0.2f));
        }
    }

    private void OnSwitchMode()
    {
        //Switches between the two modes.
        if (grappelingHookMode == Modes.Swinging)
        {
            grappelingHookMode = Modes.Grappeling;
        }
        else
        {
            grappelingHookMode = Modes.Swinging;
        }
    }

    IEnumerator Recall(float timer)
    {
        //If the hook isn't attached return the hook to the gun.
        yield return new WaitForSeconds(timer);

        Destroy(joint);

        hookRigidbody.velocity = Vector3.zero;

        hookRigidbody.isKinematic = true;
        hookCollider.enabled = false;
        hookRigidbody.useGravity = false;

        hookObject.transform.SetParent(gunObject.transform);

        hookObject.transform.localPosition = localPositionStart;
        hookObject.transform.localRotation = localRotationStart;
        hookObject.transform.localScale = localScaleStart;
    }

    //What happens if you hit a target.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            //Sets the position where the hook hit.
            isAttached = true;
            hookRigidbody.velocity = Vector3.zero;
            hookRigidbody.useGravity = false;

            //Sets a base distance between the player and hook object.
            float distanceFromPoint = Vector3.Distance(playerObject.transform.position, hookObject.transform.position);

            //Sets up the settings of the joint for swinging.
            joint = playerObject.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hookObject.transform.position;

            joint.maxDistance = distanceFromPoint * rope.maxRopeLenght;
            joint.minDistance = distanceFromPoint * rope.minRopeLenght;

            if (grappelingHookMode == Modes.Swinging) { Swinging(); }

            if (grappelingHookMode == Modes.Grappeling)
            {
                isGrappeling = true;
                isAttached = false;
                StartCoroutine(Recall(1f));
            }
        }
    }

    //Controlls swinging effect.
    private void Swinging()
    {
        joint.spring = jointSpring;
        joint.damper = jointDamper;
        joint.massScale = jointMassScale;
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
