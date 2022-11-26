using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{

  #region Fields
  [SerializeField] private GameObject EndGamePanels;
  [SerializeField] private GameObject LevelCompletedPanel;
  [SerializeField] private GameObject LevelFailedPanel;
  [SerializeField] private TMP_Text ShotCountText;
  [SerializeField] private TMP_Text ScorePercentageText;
  [SerializeField] private Image ScoreMeterImage;
  #endregion

  #region Components
  private GameManager _gameManager;
  private ObstacleManager _obstacleManager;
  #endregion

  #region Unity Functions
  private void Start()
  {
    UpdateShotCountText();
    UpdateScoreMeter();
  }

  private void OnEnable()
  {
    GameManager.OnShotHandler += UpdateShotCountText;
    GameManager.OnGameEndHandler += ActivateEndGameMenu;

    _gameManager = ManagerContainer.Instance.GetInstance<GameManager>();
    _obstacleManager = ManagerContainer.Instance.GetInstance<ObstacleManager>();
  }

  private void OnDisable()
  {
    GameManager.OnShotHandler -= UpdateShotCountText;
    GameManager.OnGameEndHandler -= ActivateEndGameMenu;
  }
  #endregion

  #region UI Functions
  public void UIF_RestartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void UIF_QuitGame()
  {
    Application.Quit();
  }

  #endregion

  #region UI Update

  public void ActivateEndGameMenu(bool condition)
  {
    EndGamePanels.SetActive(true);
    
    switch ( condition )
    {
      case false:
        LevelFailedPanel.SetActive(true);
        break;
      case true:
        LevelCompletedPanel.SetActive(true);
        break;
    }
  }

  public void UpdateShotCountText()
  {
    ShotCountText.text = $"{_gameManager.ShotCount} Shots Left";
  }

  public void UpdateScoreMeter()
  {
    ScorePercentageText.text = $"%{(int)(100*_obstacleManager.ObstaclePercentage)}";
    ScoreMeterImage.fillAmount = _obstacleManager.ObstaclePercentage;
  }
  #endregion

}
