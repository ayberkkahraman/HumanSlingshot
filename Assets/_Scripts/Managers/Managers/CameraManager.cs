using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private CinemachineVirtualCamera GameCamera;
    
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    
    [Header("Camera Shake")]
    [Space]
    [SerializeField] private float ShakeIntensity;
    [SerializeField] private float ShakeDuration;


    #region Camera
    /// <summary>
    /// Shakes the Camera
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShakeCamera()
    {
      var time = Time.time;

      _cinemachineBasicMultiChannelPerlin = GameCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

      while (Time.time < time + ShakeDuration)
      {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = ShakeIntensity;
        yield return null;
      }

      _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }
    #endregion
    
}
