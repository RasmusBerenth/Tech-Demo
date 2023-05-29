using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [Header("Rope Variables")]
    [SerializeField]
    private float _maxRopeLenght;
    public float maxRopeLenght => _maxRopeLenght;
    [SerializeField]
    private float _minRopeLenght;
    public float minRopeLenght => _minRopeLenght;

    private LineRenderer lr;
    private GrappelingHook hook;

    public Rope(float maximumRopeLenght, float minimumRopeLenght)
    {
        maximumRopeLenght = maxRopeLenght;
        minimumRopeLenght = minRopeLenght;
    }

    public void Start()
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
}
