using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LevelSavedData
{
  public int level_levelIndex;
  public int level_matchingScore;
  public int level_matchingTurn;
  public List<CardSavedData> card_savedDatas;
}

public class LevelManager : MonoBehaviour
{
  public static LevelManager instance;

  [SerializeField]
  private List<LevelDataScriptableObject> level_scriptabledatas;

  public Dictionary<int, LevelDataScriptableObject> level_datas = new Dictionary<int, LevelDataScriptableObject>();

  private int level_currentlevel = 0;

  public UnityEvent onGameStartEvent;
  public UnityEvent onGameEndingEvent;
  public bool isGameEnd = false;

  public UnityEvent onSavedSuccessEvent;

  private List<string> savedfiles = new List<string>();
  private string targetloadingfile = "";

  public LevelSavedData debug_levelSavedData;

  [SerializeField]
  private AudioSource audioSourceEffect;
  [SerializeField]
  private AudioClip gameEnding_audio;

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
    GetSavedDataFiles();
    onGameEndingEvent.AddListener(PlayAudioGameEnding);
  }

  private void CreateLevelDatas()
  {
    foreach (LevelDataScriptableObject level in level_scriptabledatas)
    {
      level_datas.Add(level.level_index, level);
    }
  }

  /*void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      StartLevelIndex(0);
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      StartLevelIndex(1);
    }
    if (Input.GetKeyDown(KeyCode.S))
    {
      SaveCurrentLevel();
    }
    if (Input.GetKeyDown(KeyCode.G))
    {
      LoadingTargetLevel();
    }

  }*/

  public void StartGame()
  {
    onGameStartEvent?.Invoke();
    StartLevelIndex(0);
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


  public void SaveCurrentLevel()
  {
    LevelSavedData levelSavedData = new LevelSavedData();

    levelSavedData.level_levelIndex = level_currentlevel;
    levelSavedData.level_matchingTurn = StageController.instance.MatchingTurn;
    levelSavedData.level_matchingScore = StageController.instance.MatchingScore;
    levelSavedData.card_savedDatas = StageController.instance.CreateStageCardSavedDatas();

    string json = JsonUtility.ToJson(levelSavedData, true);

    SavedLoadJson.SaveToJsonFile(json, "/savedlevel", level_datas[level_currentlevel].level_name + "saveddata.json");
    onSavedSuccessEvent?.Invoke();
  }

  public void LoadingTargetLevel()
  {
    var json = SavedLoadJson.LoadFromJsonFile("/savedlevel", targetloadingfile);

    if (!string.IsNullOrEmpty(json))
    {
      var loadedLevelData = JsonUtility.FromJson<LevelSavedData>(json);
      
      var leveldata = level_datas[loadedLevelData.level_levelIndex];

      onGameStartEvent?.Invoke();
      level_currentlevel = leveldata.level_index;
      AssignCardData(leveldata.level_cardDatas);
      StageController.instance.StartLoadedStage(leveldata, loadedLevelData);
      debug_levelSavedData = loadedLevelData;
    }
  }

  public void GetSavedDataFiles()
  {
    var filesname = SavedLoadJson.GetFilesNameSavedPath("/savedlevel");

    savedfiles = filesname;
    UIController.instance.SetLoadLevelSelectionPanel(savedfiles);
  }

  public void SetTargetLoadingLevel(int index)
  {
    if (savedfiles.Count > index)
    {
      targetloadingfile = savedfiles[index];
    }
  }

  void PlayAudioGameEnding()
  {
    //Debug.Log("Playing Game Ending Audio!!");
    audioSourceEffect.clip = gameEnding_audio;
    audioSourceEffect.Play();
  }

}