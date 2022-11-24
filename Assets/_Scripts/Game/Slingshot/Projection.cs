using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
  private Scene _simulationScene;
  private PhysicsScene _physicsScene;
  
  [Header("Fields")]
  [SerializeField] private Transform ObjectsHolder;
  [SerializeField] private LineRenderer LineRenderer;
  
  
  [Space]
  [Header("Trajectory")]
  [SerializeField] private TrajectoryBall TrajectoryBallPrefab;
  [SerializeField] private Material TrajectoryMaterial;
  [SerializeField] private Transform TrajectoryBallSpawnTransform;
  [Range(10f, 50f)]
  [SerializeField] private float TrajectoryBallForce = 20f;
  [SerializeField] private Vector3 TrajectoryDirection;
  [Range(5,100)]
  [SerializeField] private int MaxPhysicsFrameIterations = 25;

  [HideInInspector] public Vector3 Force => TrajectoryDirection * TrajectoryBallForce;
  

  private void Start()
  {
      CreatePhysicScene();
  }

  private void Update()
  {
    SimulateTrajectory(TrajectoryBallPrefab, TrajectoryBallSpawnTransform.position, TrajectoryDirection * TrajectoryBallForce);
  }


  private void CreatePhysicScene()
  {
    _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
    _physicsScene = _simulationScene.GetPhysicsScene();
  }

  private void SimulateTrajectory(TrajectoryBall trajectoryBallPrefab, Vector3 position, Vector3 velocity)
  {
    var fakeObject = Instantiate(trajectoryBallPrefab, position, Quaternion.identity);
    SceneManager.MoveGameObjectToScene(fakeObject.gameObject, _simulationScene);
    
    fakeObject.Init(velocity);

    LineRenderer.positionCount = MaxPhysicsFrameIterations;

    for (int i = 0; i < MaxPhysicsFrameIterations; i++)
    {
      _physicsScene.Simulate(Time.fixedDeltaTime);
      LineRenderer.SetPosition(i, fakeObject.transform.position);
    }
    
    Destroy(fakeObject.gameObject);

  }
}
