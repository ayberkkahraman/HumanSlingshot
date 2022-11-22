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
    [SerializeField] private CinemachineVirtualCamera CoinCamera;
    
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    
    [Header("Camera Follow Offset Attributes")]
    [Space]
    [SerializeField] private float FollowOffsetFactor = .5f;
    [SerializeField] private float FollowOffsetTransitionDuration = .25f;

    [Header("Camera Shake")]
    [Space]
    [SerializeField] private float ShakeIntensity;
    [SerializeField] private float ShakeDuration;

    private float _defaultFollowOffsetY;
    private float _defaultFollowOffsetZ;

    private void OnEnable()
    {
      _defaultFollowOffsetY = CoinCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y;
      _defaultFollowOffsetZ = CoinCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z;
    }

    #region Camera
  
    /// <summary>
    /// Updating the camera follow offset when the coin group increased
    /// </summary>
    /// <param name="increase"></param>
    /// <returns></returns>
    public IEnumerator UpdateCameraFollowOffset(bool increase)
    {
        if (_defaultFollowOffsetY >= CoinCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y &&
            _defaultFollowOffsetZ >= CoinCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z &&
          increase == false)
        {
          yield break;
        }

        //Scaling condition checker fot upward or downward
        int multiplier = increase == true ? 1 : -1;

        float followOffsetY = FollowOffsetFactor;
        float followOffsetZ = -FollowOffsetFactor;
        
        var time = Time.time;

        while (Time.time - time < FollowOffsetTransitionDuration)
        {
          float currentFollowOffsetY = CoinCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y;
          float currentFollowOffsetZ = CoinCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z;
          
          CoinCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = 
            Mathf.MoveTowards(currentFollowOffsetY, currentFollowOffsetY + followOffsetY*multiplier, Time.deltaTime/(FollowOffsetTransitionDuration*2));
          
          CoinCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z =
            Mathf.MoveTowards(currentFollowOffsetZ, currentFollowOffsetZ + followOffsetZ*multiplier, Time.deltaTime/(FollowOffsetTransitionDuration*2));
          
          yield return null;
        }
    }

    /// <summary>
    /// Switches the main camera to given virtual camera
    /// </summary>
    /// <param name="targetCamera"></param>
    public void SwitchCamera(CinemachineVirtualCamera targetCamera)
    {
      FindObjectsOfType<CinemachineVirtualCamera>().ToList().ForEach(x => x.m_Priority = 9);
      targetCamera.m_Priority = 10;
    }
    
    /// <summary>
    /// Shakes the Camera
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShakeCamera()
    {
      var time = Time.time;

      _cinemachineBasicMultiChannelPerlin = CoinCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

      while (Time.time < time + ShakeDuration)
      {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = ShakeIntensity;
        yield return null;
      }

      _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }
    #endregion
    
}
