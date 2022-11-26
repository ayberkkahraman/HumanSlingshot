using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace _Scripts.SubSystem.Extensions
{
  public static class Extensions
  {
      #region Component Systems
      /// <summary>
      /// Checks if the GameObject has Component or not
      /// </summary>
      /// <param name="gameObject"></param>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      public static bool HasComponent<T>(this GameObject gameObject) where T : Component
      {
        return gameObject.GetComponent<T>() != null;
      }
        
      /// <summary>
      /// Checks if the Collider has Component or not
      /// </summary>
      /// <param name="gameObject"></param>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      public static bool HasComponent<T>(this Collider gameObject) where T : Component
      {
        return gameObject.GetComponent<T>() != null;
      }
      
      /// <summary>
      /// Checks if the Transform has Component or not
      /// </summary>
      /// <param name="gameObject"></param>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      public static bool HasComponent<T>(this Transform gameObject) where T : Component
      {
        return gameObject.GetComponent<T>() != null;
      }
      #endregion
    
      #region LayerMask Systems
      /// <summary>
      /// Returns a string array of layer names from a LayerMask.
      /// </summary>
      public static string[] MaskToNames(this LayerMask original)
      {
        var output = new List<string>();
			  
        for (int i = 0; i < 32; ++i)
        {
          int shifted = 1 << i;
          if ((original & shifted) == shifted)
          {
            string layerName = LayerMask.LayerToName(i);
            if (!string.IsNullOrEmpty(layerName))
            {
              output.Add(layerName);
            }
          }
        }
        return output.ToArray();
      }
      #endregion
  }
}

