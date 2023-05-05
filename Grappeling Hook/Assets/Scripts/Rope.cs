using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    private float _maxRopeLength = 100f;
    public float maxRopeLength { get { return _maxRopeLength; } set { _maxRopeLength = value; } }

    [SerializeField]
    private float _minRopeLength = 0f;
    public float minRopeLenght { get { return _minRopeLength; } set { _minRopeLength = value; } }

    private float _currentRopeLenght;
    public float currentRopeLenght { get { return _currentRopeLenght; } set { _currentRopeLenght = value; } }


    public void AlterRopeLenght()
    {

    }
}
