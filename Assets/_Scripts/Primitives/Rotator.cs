using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
  [SerializeField] private Vector3 Direction;
  [SerializeField] private float Speed;
  
  private void Update()
  {
    transform.Rotate(Direction * (Speed * Time.deltaTime), Space.World);
  }
}
