using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [Header("Rope Variables")]
    private float currentRopeLenght;

    LineRenderer lr;
    GrappelingHook hook;

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

    public void AlterRopeLenght()
    {

    }
}
