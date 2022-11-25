using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    #region Fields
    
  #endregion

  #region Components
  private Animator _animator;
  private Rigidbody _rigidbody;
  private Collider _collider;
  #endregion

  #region Animation Hashing
  private static readonly int Jump = Animator.StringToHash("Jump");
  #endregion

  #region Unity Functions
  private void OnEnable()
  {
    _animator = GetComponent<Animator>();
    _rigidbody = GetComponent<Rigidbody>();
    _collider = GetComponent<Collider>();
  }
  #endregion
    
    
    public void Throw(Projection projection)
    {
      transform.parent = null;
      
      _rigidbody.isKinematic = false;
      _collider.isTrigger = false;
  
      _rigidbody.AddForce(projection.Force, ForceMode.Impulse);

      _animator.applyRootMotion = false;
      _animator.SetTrigger(Jump); 
    }
}
