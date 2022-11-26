using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HumanSelectionManager : MonoBehaviour
{
  [HideInInspector] public List<Human> Humans;
  private Human _nextHuman;

  private void Start()
  {
    Humans = FindObjectsOfType<Human>().ToList();
  }

  public void ChangeHuman()
  {
    if (Humans.Exists(x => x.HumanState != Human.State.Throwed))
    {
      _nextHuman = Humans.Find(x => x.HumanState != Human.State.Throwed);
      
      _nextHuman.HumanState = Human.State.Walking;
      _nextHuman.ChangeState();
    }
    else
    {
      return;
    }
    

  }
}
