using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
  public static LevelManager instance;

  [SerializeField]
  private List<LevelDataScriptableObject> level_scriptabledatas;

  public Dictionary<int, LevelDataScriptableObject> level_datas = new Dictionary<int, LevelDataScriptableObject>();

  private int level_currentlevel = 0;

  public UnityEvent onGameEndingEvent;
  public bool isGameEnd = false;

  private void Awake()
  {
    if(instance != null)
    {
      return;
    }
    instance = this;
  }

  void Start()
  {
    CreateLevelDatas();
  }

  private void CreateLevelDatas()
  {
    foreach (LevelDataScriptableObject level in level_scriptabledatas)
    {
      level_datas.Add(level.level_index, level);
    }
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      StartLevelIndex(0);
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      StartLevelIndex(1);
    }
  }

  public void StartLevelIndex(int index)
  {
    if(level_datas.ContainsKey(index))
    {
      var level = level_datas[index];
      level_currentlevel = level.level_index;

      Debug.Log("Starting Level: " + level.level_name);
      if (level != null)
      {
        AssignCardData(level.level_cardDatas);
        StageController.instance.StartStage(level);
      }
    }
  }

  public void StartNextLevel()
  {
    level_currentlevel++;
    if(level_scriptabledatas.Count > level_currentlevel)
    {
      StartLevelIndex(level_currentlevel);
    }
  }

  public bool CheckingGameEnding()
  {  
    int nextLevelIndex = level_currentlevel + 1;

    if (!level_datas.ContainsKey(nextLevelIndex))
    {
      isGameEnd = true;
    }
    return isGameEnd;
  }

  void AssignCardData(List<CardDataScriptableObject> newCardDatas)
  {
    var cardSpawner = CardSpawner.instance;

    cardSpawner.cardDatas.Clear();
    cardSpawner.cardDatas = new List<CardDataScriptableObject>(newCardDatas);
  }

}