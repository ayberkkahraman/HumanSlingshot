using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private int GameCompletePercentageCondition = 80;
    private int _shotCount;
    public int ShotCount
    {
        get => _shotCount;
        set => _shotCount = value;
    }

    public delegate void OnShot();
    public static OnShot OnShotHandler;

    public delegate void OnGameEnd(bool state);
    public static OnGameEnd OnGameEndHandler;

    public enum State
    {
        Running,
        Finished
    }

    public State GameState;


    #region Init / DeInit
    public void Init()
    {
        OnShotHandler += UpdateShotCount;

        ShotCount = 2;

        GameState = State.Running;
    }

    public void DeInit()
    {                     
        OnShotHandler -= UpdateShotCount;    
    }
    #endregion

    #region Unity Functions
    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
       DeInit();
    }

    private void Update()
    {
        if (ShotCount != 0)
            return;
        if (ObstacleManager.IsGameStable().Invoke() != true)
            return;
        if(GameState == State.Finished)
            return;;

        
        bool endCondition = (int)(ManagerContainer.Instance.GetInstance<ObstacleManager>().ObstaclePercentage*100) >= GameCompletePercentageCondition;

        OnGameEndHandler(endCondition);

        GameState = State.Finished;
    }
    #endregion

    #region Shot Configuration
    private void UpdateShotCount()
    {
        ShotCount--;
    }
  #endregion

}
