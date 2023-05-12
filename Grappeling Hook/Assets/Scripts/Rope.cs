using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [Header("Rope Variables")]
    [SerializeField]
    private float _maxRopeLength = 100f;
    public float maxRopeLength { get { return _maxRopeLength; } set { _maxRopeLength = value; } }

    [SerializeField]
    private float _minRopeLength = 0f;
    public float minRopeLenght { get { return _minRopeLength; } set { _minRopeLength = value; } }

    private float _currentRopeLenght;
    public float currentRopeLenght { get { return _currentRopeLenght; } set { _currentRopeLenght = value; } }

    LineRenderer lr;
    GrappelingHook hook;


    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        hook = GetComponent<GrappelingHook>();
    }

    private void LateUpdate()
    {
        //Renders the rope.
        DrawRope();
    }

    public void DrawRope()
    {
        //Set the two ends of the rope.
        lr.SetPosition(0, hook.ropeStartPoint.transform.position);
        lr.SetPosition(1, hook.hookObject.transform.position);
    }

    public void AlterRopeLenght()
    {

    }
}
