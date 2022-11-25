using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlingshotController : MonoBehaviour
{
  #region Fields

  [SerializeField] private Collider SlingshotPuller;
  [SerializeField] private Human Human;

  [Space]
  [Range(.5f,1.25f)]
  [SerializeField] private float PullForceVerticalLimit = 1.0f;
  [Range(.25f,.5f)]
  [SerializeField] private float PullForceHorizontalLimit = .35f;
  [Range(.0f, .2f)]
  [SerializeField] private float SlingPositionResetDuration = .075f;

  private Vector3 _defaultSlingshotPosition;
  private Vector3 _defaultSlingshotRotation;
  
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
    Init();
  }
  private void Update()
  {
    InputConfiguration();
  }
  #endregion


  #region Init
  private void Init()
  {
    _camera = Camera.main;

    _projection = GetComponent<Projection>();

    var slingshotPullerTransform = SlingshotPuller.transform;
    _defaultSlingshotPosition = slingshotPullerTransform.position;
    _defaultSlingshotRotation = slingshotPullerTransform.localEulerAngles;
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

  /// <summary>
  /// Executes when a finger has touched to the screen
  /// </summary>
  private void OnFingerBegan()
  {
            
    Ray ray = _camera.ScreenPointToRay(Input.touches[0].position);
    
    if (!Physics.Raycast(ray, out var raycastHit, 100f))
      return;
    
    //Checks if the touch is on the sling or not 
    if (raycastHit.collider == SlingshotPuller)
    {
      _pullingSling = true;
      _lastFingerPosition = Input.touches[0].position;
    }

    else { _pullingSling = false; }

  }

  /// <summary>
  /// Executes when the touch has been moved
  /// </summary>
  private void OnFingerMoved()
  {
    _moveFactorX = Input.touches[0].position.x - _lastFingerPosition.x;
    _moveFactorY = Input.touches[0].position.y - _lastFingerPosition.y;
  }
  
  /// <summary>
  /// Executes when the finger has released 
  /// </summary>
  private void OnFingerReleased()
  {
    _moveFactorX = 0f;
    _moveFactorY = 0f;
    
    if (_pullingSling)
    {
        Human.Throw(_projection);
    }
    
    SlingshotPuller.transform.DOMove(_defaultSlingshotPosition, SlingPositionResetDuration);
    SlingshotPuller.transform.DORotate(_defaultSlingshotRotation + Vector3.up*90f, SlingPositionResetDuration);

    //Reset the trajectory simulation
    _projection.ResetTrajectoryForce();

    _pullingSling = false;
  }
  
  #endregion

  /// <summary>
  /// Executes the 
  /// </summary>
  private void PullSling()
  {
    //-------------------------------------------------POSITION-------------------------------------------------\\
    //Get target positions based on finger position
    var pullPositionX = (Vector3.right * _moveFactorX / 1000).x;
    var pullPositionZ = (Vector3.forward * _moveFactorY / 500).z;

    //Sets the limit for positions
    pullPositionX = Mathf.Clamp(pullPositionX, -PullForceHorizontalLimit, PullForceHorizontalLimit);
    pullPositionZ = Mathf.Clamp(pullPositionZ, -PullForceVerticalLimit, PullForceVerticalLimit);

    #region Applied Trajectory Simulation
    //Apply force to the trajectory simulation

    var appliedForce = (int)(Mathf.Abs(pullPositionZ * 5f));
    _projection.UpdateTrajectoryForce(appliedForce);
    //------------------------------------------------------------------------------

    #endregion

    //Setting new position visualizing the pull
    var slingPosition = _defaultSlingshotPosition + new Vector3(pullPositionX, 0f, pullPositionZ);
    
    //Assign new position
    SlingshotPuller.transform.position = slingPosition;
    //-----------------------------------------------------------------------------------------------------------\\
    
    
    //-------------------------ROTATION-------------------------\\
    var targetAngle = -pullPositionX * 25f;

    var slingRotation = new Vector3(
      _defaultSlingshotRotation.x,
      targetAngle + _defaultSlingshotRotation.y,
      _defaultSlingshotRotation.z
    );

    SlingshotPuller.transform.localEulerAngles = slingRotation;
    //----------------------------------------------------------\\
  }
  
}
