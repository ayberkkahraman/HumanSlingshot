using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlingshotController : MonoBehaviour
{
  #region Fields

  [SerializeField] private Collider SlingCollider;
  [SerializeField] private Human Human;

  [Space]
  [Range(.5f,1.25f)]
  [SerializeField] private float PullForceVerticalLimit = 1.0f;
  [Range(.25f,.5f)]
  [SerializeField] private float PullForceHorizontalLimit = .35f;
  [Range(.0f, .2f)]
  [SerializeField] private float SlingPositionResetDuration = .075f;

  private Vector3 _defaultSlingshotPosition;
  
  private Vector2 _lastFingerPosition;
  private float _moveFactorX;
  private float _moveFactorY;

  private bool _pullingSling;
  #endregion

  #region Components
  private Camera _camera;
  private Projection _projection;
  
  #endregion
  
  


  #region Unity Functions
  private void OnEnable()
  {
    _camera = Camera.main;

    _projection = GetComponent<Projection>();
    
    _defaultSlingshotPosition = SlingCollider.transform.position;
  }
  private void Update()
  {
    InputConfiguration();
  }
  #endregion
  
  
  
  
  
  
    #region Input
  /// <summary>
  /// Updates the swerving inputs
  /// </summary>
  private void InputConfiguration()
  {
    //Checks If the player is touching the screen or not
    if (Input.touchCount <= 0)return;
        
        
    switch ( Input.touches[0] )
    {
      case { phase: TouchPhase.Began }:
        OnFingerBegan();
        break;
      case { phase: TouchPhase.Moved }:
        
        if (_pullingSling == false) return;
        
        OnFingerMoved();
        PullSling();
        
        break;
      case { phase: TouchPhase.Ended }:
        OnFingerReleased();
        break;
      case { phase: TouchPhase.Canceled }:
        OnFingerReleased();
        break;
      case { phase: TouchPhase.Stationary }:
        break;
      default:
        return;
    }
  }

  private void OnFingerBegan()
  {
            
    Ray ray = _camera.ScreenPointToRay(Input.touches[0].position);
    if (Physics.Raycast(ray, out var raycastHit, 100f))
    {
      //Checks if the touch is on the sling or not 
      if (raycastHit.collider == SlingCollider)
      {
        _pullingSling = true;
        _lastFingerPosition = Input.touches[0].position;
      }

      else { _pullingSling = false; }
    }

  }

  private void OnFingerMoved()
  {
    _moveFactorX = Input.touches[0].position.x - _lastFingerPosition.x;
    _moveFactorY = Input.touches[0].position.y - _lastFingerPosition.y;
  }
  
  private void OnFingerReleased()
  {
    _moveFactorX = 0f;
    _moveFactorY = 0f;
    
    if (_pullingSling)
    {
        Human.Throw(_projection);
    }
      
    
    SlingCollider.transform.DOMove(_defaultSlingshotPosition, SlingPositionResetDuration);
    
    _pullingSling = false;
  }
  
  #endregion

  /// <summary>
  /// Executes the 
  /// </summary>
  private void PullSling()
  {
    //Get target positions based on finger position
    var pullPositionX = (Vector3.right * _moveFactorX / 1000).x;
    var pullPositionZ = (Vector3.forward * _moveFactorY / 500).z;

    //Sets the limit for positions
    pullPositionX = Mathf.Clamp(pullPositionX, -PullForceHorizontalLimit, PullForceHorizontalLimit);
    pullPositionZ = Mathf.Clamp(pullPositionZ, -PullForceVerticalLimit, PullForceVerticalLimit);
    
    //Setting new position visualizing the pull
    var slingPosition = _defaultSlingshotPosition + new Vector3(pullPositionX, 0f, pullPositionZ);
    
    //Assign new position
    SlingCollider.transform.position = slingPosition;
  }
  
}
