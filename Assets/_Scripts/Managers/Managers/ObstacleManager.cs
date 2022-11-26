using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.SubSystem.Extensions;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
  [SerializeField] private List<Transform> ObstacleParents;

  private List<Rigidbody> _rigidbodies = new List<Rigidbody>();
  private Collider[] _colliders;

  public Transform ExplosionCenter;
  [Range(2.5f, 20.0f)][SerializeField] private float ExplosionRadius = 5f;
  [Range(20f, 150f)][SerializeField] private float ExplosionForce = 100f;
  public List<Rigidbody> ObstacleRigidbodies
  {
    get => _rigidbodies;
    set
    {
      ObstacleRigidbodies = value;
    }
  }

  public static List<Rigidbody> Obstacles;
  
  
  public int ObstaclesCount => ObstacleRigidbodies.Count;

  [HideInInspector] public int DefaultObstaclesCount;
  
  public float ObstaclePercentage => (float)(DefaultObstaclesCount - ObstaclesCount) / DefaultObstaclesCount;

  public static Func<bool> IsGameStable() => () =>
  {
    return Obstacles.All(x =>
           {
             Vector3 velocity;
             return (velocity = x.velocity).x <= .03f && velocity.y <= .03f && velocity.z <= .03f;
           }) && 
           ManagerContainer.Instance.GetInstance<HumanSelectionManager>().Humans.Count == 0;
  };

  public Action ActivateObstacles() => () =>
  {
    foreach (var obstacleRigidbody in ObstacleRigidbodies)
    {
      StartCoroutine(ManagerContainer.Instance.GetInstance<CameraManager>().ShakeCamera());
      obstacleRigidbody.isKinematic = false;
    }
  };
  public void Start()
  {
    foreach (Transform obstacleParent in ObstacleParents)
    {
      foreach (Transform obstacle in  obstacleParent)
      {
        if(obstacle.HasComponent<Rigidbody>()){ObstacleRigidbodies.Add(obstacle.GetComponent<Rigidbody>());}
      }
    }

    DefaultObstaclesCount = ObstaclesCount;
    Obstacles = ObstacleRigidbodies;
  }

  public void Explode(float explosionMultiplier, Vector3 explosionCenter)
  {
    var size = Physics.OverlapSphereNonAlloc(explosionCenter, ExplosionRadius, _colliders);

    for(int i = 0; i < size; i++)
    {
      var obstacle = _colliders[i];
      if (obstacle.HasComponent<Rigidbody>())
      {
        obstacle.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce * explosionMultiplier, obstacle.transform.position, ExplosionRadius/5f);
      }
    }
  }
  
}
