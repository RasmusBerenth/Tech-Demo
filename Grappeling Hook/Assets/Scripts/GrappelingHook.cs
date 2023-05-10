using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappelingHook : Rope
{
    private PlayerControlls controlls;
    private Rigidbody hookRigidbody;
    private SphereCollider hookCollider;

    [Header("Grappelinghook Variables")]
    [SerializeField]
    private GameObject gunObject;
    [SerializeField]
    private GameObject hookObject;
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
        transform.parent = null;

        Vector3 shootDirection = gunObject.transform.localPosition;

        hookRigidbody.AddForce(Vector3.forward.normalized * hookSpeed * Time.deltaTime, ForceMode.Impulse);
        hookCollider.enabled = true;
        hookRigidbody.useGravity = true;

        StartCoroutine(Recall());
    }

    private void OnSwitchMode()
    {

    }

    IEnumerator Recall()
    {
        yield return new WaitForSeconds(5);

        hookCollider.enabled = false;
        hookRigidbody.useGravity = false;

        transform.position = new Vector3(0, 1, 0);
        hookRigidbody.velocity = Vector3.zero;
    }

    private void Grapple()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
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
