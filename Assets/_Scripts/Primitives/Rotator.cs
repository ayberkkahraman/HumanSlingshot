using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 Direction = Vector3.up;
    
    [Range(5, 50)]
    [SerializeField] private int Speed = 25;

    private void Update()
    {
        transform.Rotate(Direction * (Speed * Time.deltaTime), Space.World);
    }
}
