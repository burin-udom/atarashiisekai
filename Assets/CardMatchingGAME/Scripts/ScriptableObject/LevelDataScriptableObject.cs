using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 2)]
public class LevelDataScriptableObject : ScriptableObject
{
  public string level_name = "level01";
  public int level_index;
  public int level_cardspawnRow = 1;
  public int level_cardspawnColumn = 1;
  public int level_delaycardflip = 1000;
  public int level_cardmatchingcount = 2;
  public int level_scoretopasslevel = 5;
  public List<CardDataScriptableObject> level_cardDatas;
}
