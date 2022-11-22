using _Scripts.SubSystems.Behaviours;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    
    #region Save / Load

  /// <summary>
  /// Save data with given key value
  /// </summary>
  public void SaveData<T>(string key, T saveData)
  {
    string gameDataJson = DataBehaviour.Serialize<T>(saveData);
    PlayerPrefs.SetString(key, gameDataJson);
  }

  /// <summary>
  /// Load data with save key saved earlier
  /// </summary>
  public T LoadData<T>(string key)
  {
    string gameDataJson = PlayerPrefs.GetString(key);
    return DataBehaviour.DeSerialize<T>(gameDataJson);
  }

    #endregion
}
