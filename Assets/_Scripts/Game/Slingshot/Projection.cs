using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
  private Scene _simulationScene;
  private PhysicsScene _physicsScene;

  private const string Simulation = ""; // The name of the Simulation Scene
  
  [Header("Fields")]
  [SerializeField] private LineRenderer LineRenderer;


  [Space]
  [Header("Trajectory")]
  [SerializeField] private TrajectoryVisualizer TrajectoryVisualizerPrefab;
  [SerializeField] private Transform TrajectoryBallSpawnTransform;
  
  [Range(10f, 50f)]
  [SerializeField] private float TrajectoryBallForce = 20f;

  public float TrajectoryForce
  {
    get => TrajectoryBallForce;
    set => TrajectoryBallForce = value;
  }
  
  
  [Range(.0f,1f)]
  [SerializeField] private float TrajectoryHeight = .5f;
  public float TrajectoryHeightForce
  {
    get => TrajectoryHeight;
    set => TrajectoryHeight = value;
  }
  
  [Range(5,100)]
  [SerializeField] private int MaxPhysicsFrameIterations = 25;

  public int PhysicsFrameIterations
  {
    get => MaxPhysicsFrameIterations;
    set => MaxPhysicsFrameIterations = value;
  }
  
  [HideInInspector] public int DefaultIterationsCount;
  [HideInInspector] public float DefaultTrajectoryHeight;
  [HideInInspector] public float DefaultTrajectoryForce;
  

  [HideInInspector] public Vector3 Force => TrajectoryBallSpawnTransform.rotation * (Vector3.forward - Vector3.right*TrajectoryHeightForce) * TrajectoryForce;
  

  private void Start()
  {
      DefaultIterationsCount = PhysicsFrameIterations;
      DefaultTrajectoryHeight = TrajectoryHeightForce;
      DefaultTrajectoryForce = TrajectoryForce;
      
      CreatePhysicScene();
  }

  private void Update()
  {
    SimulateTrajectory(TrajectoryVisualizerPrefab, TrajectoryBallSpawnTransform.position, Force);
  }


  private void CreatePhysicScene()
  {
    _simulationScene = SceneManager.CreateScene(nameof(Simulation), new CreateSceneParameters(LocalPhysicsMode.Physics3D));
    _physicsScene = _simulationScene.GetPhysicsScene();
  }

  private void SimulateTrajectory(TrajectoryVisualizer trajectoryVisualizerPrefab, Vector3 position, Vector3 velocity)
  {
    var visualizerObject = Instantiate(trajectoryVisualizerPrefab, position, Quaternion.identity);
    SceneManager.MoveGameObjectToScene(visualizerObject.gameObject, _simulationScene);
    
    visualizerObject.Init(velocity);

    LineRenderer.positionCount = PhysicsFrameIterations;

    for (int i = 0; i < PhysicsFrameIterations; i++)
    {
      _physicsScene.Simulate(Time.fixedDeltaTime);
      LineRenderer.SetPosition(i, visualizerObject.transform.position);
    }

    Destroy(visualizerObject.gameObject);

  }

  #region Trajectory Configuration
  /// <summary>
  /// Updates the forces when the player has pulled the slingshot
  /// </summary>
  /// <param name="appliedForce"></param>
  public void UpdateTrajectoryForce(int appliedForce)
  {
    PhysicsFrameIterations = DefaultIterationsCount + appliedForce;
    TrajectoryHeightForce = DefaultTrajectoryHeight + appliedForce / 1000f;
    TrajectoryForce = DefaultTrajectoryForce + appliedForce/2f;
  }
  
  /// <summary>
  /// Resets forces to default when the player has released the slingshot bend
  /// </summary>
  public void ResetTrajectoryForce()
  {
    PhysicsFrameIterations = DefaultIterationsCount;
    TrajectoryHeightForce = DefaultTrajectoryHeight;
    TrajectoryForce = DefaultTrajectoryForce;
  }
  #endregion

}
