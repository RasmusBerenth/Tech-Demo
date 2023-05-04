using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappelingHook : MonoBehaviour
{
    PlayerControlls controlls;

    private void Awake()
    {
        controlls = new PlayerControlls();


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
