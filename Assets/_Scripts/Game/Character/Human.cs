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
  #endregion

  #region Animation Hashing
  private static readonly int Jump = Animator.StringToHash("Jump");
  #endregion

  #region Unity Functions
  private void OnEnable()
  {
    _animator = GetComponent<Animator>();
    _rigidbody = GetComponent<Rigidbody>();
  }
  #endregion
    
    
    public void Throw(Projection projection)
    {     
      _rigidbody.isKinematic = false;
      _rigidbody.AddForce(projection.Force, ForceMode.Impulse);
      
       _animator.SetTrigger(Jump); 
    }
}
