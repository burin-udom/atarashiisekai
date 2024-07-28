using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  public static LevelManager instance;

  [SerializeField]
  private List<LevelDataScriptableObject> level_scriptabledatas;

  public Dictionary<int, LevelDataScriptableObject> level_datas = new Dictionary<int, LevelDataScriptableObject>();

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
    var level = level_scriptabledatas[index];

    if(level != null)
    {
      AssignCardData(level.level_cardDatas);
      StageController.instance.StartStage(level);
    }
  }

  void AssignCardData(List<CardDataScriptableObject> newCardDatas)
  {
    var cardSpawner = CardSpawner.instance;

    cardSpawner.cardDatas.Clear();
    cardSpawner.cardDatas = new List<CardDataScriptableObject>(newCardDatas);
  }

}