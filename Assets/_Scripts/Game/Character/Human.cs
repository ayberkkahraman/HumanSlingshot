using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Human : MonoBehaviour
{
    #region Fields
    public enum State
    {
      Waiting,
      Walking,
      Climbing,
      Ready,
      Throwed
    };

    public State HumanState;
    [SerializeField] private Transform MovePoint;
    [SerializeField] private Transform ClimbPoint;
    [SerializeField] private Transform SlingParent;

    private bool _hasArrivedToTarget;
    private bool _hasTriggeredExplosion;


    #endregion

  #region Components
  private Animator _animator;
  private Rigidbody _rigidbody;
  private Collider _collider;
  private SlingshotController _slingshotController;
  private HumanSelectionManager _humanSelectionManager;
  #endregion

  #region Constraints

  private const string OBSTACLE_LAYER = "Obstacle";
  #endregion

  #region Animation Hashing
  private static readonly int Jump = Animator.StringToHash("Jump");
  private static readonly int Climb = Animator.StringToHash("Climb");
  private static readonly int IsMoving = Animator.StringToHash("IsRunning");
  #endregion

  #region Unity Functions
  private void OnEnable()
  {
      Init();
  }

  private void OnCollisionEnter(Collision collision)
  {
    string layerOfTheCollider = LayerMask.LayerToName(collision.gameObject.layer);
    
    //Checks if the collision layer is an obstacle or not
    if (layerOfTheCollider != OBSTACLE_LAYER) return;

    ObstacleManager obstacleManager = ManagerContainer.Instance.GetInstance<ObstacleManager>();

    if(_humanSelectionManager.Humans.Contains(this))
      _humanSelectionManager.Humans.Remove(this);

    //Disabling rigidbody kinematics
    if (_hasArrivedToTarget == false)
    {
      obstacleManager.Explode(2.5f, collision.collider.ClosestPoint(transform.position));
      obstacleManager.ActivateObstacles()?.Invoke();
      _hasArrivedToTarget = true;
    }

    //Checks if the human is in red zone 
    if (collision.gameObject.CompareTag($"RedCube") != true) return;
    
    //It allows human to be not affect easily from the explosion
    _rigidbody.mass = 1000;

    
    //Prevents multiple explosions if the Human collides with the red zone after the first explosion
    if (_hasTriggeredExplosion == false)
    {
      obstacleManager.Explode(5, obstacleManager.ExplosionCenter.position);
      _hasTriggeredExplosion = true;
    }
    
    
    this.enabled = false;

  }
  #endregion

  #region Init / DeInit
  private void Init()
  {
    _animator = GetComponent<Animator>();
    _rigidbody = GetComponent<Rigidbody>();
    _collider = GetComponent<Collider>();
    
    _slingshotController = FindObjectOfType<SlingshotController>();
    _humanSelectionManager = ManagerContainer.Instance.GetInstance<HumanSelectionManager>();
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

      HumanState = State.Throwed;

      //Assign next human to be throwed from sling
      _humanSelectionManager.ChangeHuman();
      
      //Update Remaining Shot count
      GameManager.OnShotHandler();

    }

    public void ChangeState()
    {
      switch ( HumanState )
      {
         case State.Walking:

           _animator.SetBool(IsMoving, true);
           
           var targetMovePosition = MovePoint.position;
           
           transform.DORotate(targetMovePosition + Vector3.up*-90f, .25f);
           transform.DOMove(targetMovePosition, .5f)
             .OnComplete(() =>
             {
               _animator.SetBool(IsMoving, false);
               
               HumanState = State.Climbing;
               
               transform.DORotate(MovePoint.position, 1f);
               
               ClimbToSling(ClimbPoint);
               
             });
           break;;
      }                                                 
    }

    public void ClimbToSling(Transform climbTransform)
    {
      _animator.SetTrigger(Climb);
      
      transform.DOMove(climbTransform.position, 1.75f)
        .OnComplete(() =>
        {
          HumanState = State.Ready;
          
          //Setting human to be ready
          transform.parent = SlingParent;
          _slingshotController.Human = this;
        });
      
    }

    public void ReadyToThrow()
    {
      HumanState = State.Ready;
    }
}
