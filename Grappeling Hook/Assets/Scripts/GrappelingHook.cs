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

    public GameObject hookObject;
    public GameObject ropeStartPoint;
    public GameObject playerObject;

    [SerializeField]
    private float hookSpeed;
    [SerializeField]
    private float grappelingSpeed;
    [SerializeField]
    float jointSpring;
    [SerializeField]
    float jointDamper;
    [SerializeField]
    float jointMassScale;

    private Vector3 localPositionStart;
    private Quaternion localRotationStart;

    private SpringJoint joint;
    private Rope rope;

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
        localPositionStart = transform.localPosition;
        localRotationStart = transform.localRotation;
    }

    private void Update()
    {
        if (isAttached) { StopAllCoroutines(); }
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
            StartCoroutine(Recall(1.5f));

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
        else
        {
            grappelingHookMode = Modes.Swinging;
            Debug.Log(grappelingHookMode);
        }

    }

    IEnumerator Recall(float timer)
    {
        yield return new WaitForSeconds(timer);

        Destroy(joint);

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
            //Sets the position where the hook hit.
            isAttached = true;
            hookRigidbody.velocity = Vector3.zero;
            hookRigidbody.useGravity = false;

            if (grappelingHookMode == Modes.Swinging) { Swinging(); }

            if (grappelingHookMode == Modes.Grappeling) { Grappeling(); }
        }
    }

    private void Swinging()
    {
        //Sets a base distance between the player and hook object.
        float distanceFromPoint = Vector3.Distance(playerObject.transform.position, hookObject.transform.position);

        //Sets up the settings of the joint for swinging.
        joint = hookObject.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = hookObject.transform.position;
        //joint.connectedBody = playerObject.GetComponent<Rigidbody>();

        joint.maxDistance = distanceFromPoint * rope.maxRopeLenght;
        joint.minDistance = distanceFromPoint * rope.minRopeLenght;

        joint.spring = jointSpring;
        joint.damper = jointDamper;
        joint.massScale = jointMassScale;
    }

    private void Grappeling()
    {

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
