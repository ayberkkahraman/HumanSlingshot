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
  [SerializeField] private GameObject EndGameMenu;
  public TMP_Text CurrencyText;
  #endregion

  #region Unity Functions
  private void Start()
  {

  }
  #endregion

  #region UI Functions
  public void UIF_StartGame()
  {

  }

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
  public void ActivateEndGameMenu()
  {
    EndGameMenu.SetActive(true);
  }
  #endregion

}
