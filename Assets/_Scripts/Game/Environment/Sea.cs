using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.SubSystem.Extensions;
using UnityEngine;

public class Sea : MonoBehaviour
{
  private const string OBSTACLE_LAYER = "Obstacle";
  

  private void OnTriggerEnter(Collider collision)
  {

    if (collision.HasComponent<Human>())
    {
      if(ManagerContainer.Instance.GetInstance<HumanSelectionManager>().Humans.Contains(collision.GetComponent<Human>()))
        ManagerContainer.Instance.GetInstance<HumanSelectionManager>().Humans.Remove(collision.GetComponent<Human>());
    }
    
    string layerOfTheCollider = LayerMask.LayerToName(collision.gameObject.layer);
    
    //Checks if the collision layer is an obstacle or not
    if (layerOfTheCollider != OBSTACLE_LAYER) return;

    ManagerContainer.Instance.GetInstance<ObstacleManager>().ObstacleRigidbodies.Remove(collision.GetComponent<Rigidbody>());
    ObstacleManager.Obstacles = ManagerContainer.Instance.GetInstance<ObstacleManager>().ObstacleRigidbodies;
    ManagerContainer.Instance.GetInstance<UIManager>().UpdateScoreMeter();
    
    Destroy(collision.gameObject, 2f);

  }

}
