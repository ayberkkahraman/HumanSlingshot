using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryBall : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 velocity)
    {
        
        _rigidbody.AddForce(velocity, ForceMode.Impulse);
    }
}
