using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField, Tooltip("deg/s")] private float _angularSpeed = 360f;
    [SerializeField] private Space _space = Space.Self;

    private void Update()
    {
        transform.Rotate(Vector3.up, _angularSpeed * Time.deltaTime, _space);
    }
}
