using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ManagerContainer : MonoBehaviour{
  #region Childs
  public static ManagerContainer Instance;

  public List<MonoBehaviour> Managers;
  #endregion

  #region Singleton
  private void Awake()
  {
    if (Instance == null) Instance = this;
    else { Destroy(Instance); }
  }
  #endregion



  // ReSharper disable Unity.PerformanceAnalysis
  /// Get Instance for singleton access
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  public T GetInstance<T>() where T : MonoBehaviour
  {
    //CHECKS IF THE MANAGERS LIST CONTAINS THE "T" INSTANCE
    if (Managers.Exists(x => x as T))
    {
      //FINDS THE INSTANCE FOR ASSIGNING TO ACCESS
      return Managers.Find(x => x as T) as T;
    }
    else { Debug.LogError($"Managers list not contains the **{typeof(T)}**");
      return null;
    }
  }
}
