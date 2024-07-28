using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
  public static UIController instance;

  public Text text_matchingscoreDisplay;
  public Text text_matchingturnDisplay;

  public GameObject toggle_selectloadlevel;
  public Transform transform_selectloadlevelparent;
  public ToggleGroup togglegroup_selectloadlevel;

  private List<GameObject> togglelist_levelselection = new List<GameObject>();

  private void Awake()
  {
    if(instance != null)
    {
      return;
    }
    instance = this;
  }

  public void DisplayMatchingScore(int score)
  {
    text_matchingscoreDisplay.text = score.ToString();
  }
  public void DisplayMatchingTurn(int turn)
  {
    text_matchingturnDisplay.text = turn.ToString();
  }

  public void SetLoadLevelSelectionPanel(List<string> levelsname)
  {
    ClearToggleLevelSelectionList();
    togglelist_levelselection = new List<GameObject>();

    int i = 0;
    foreach(string levelname in levelsname)
    {
      var toggleselectlevel = Instantiate(toggle_selectloadlevel, transform_selectloadlevelparent);

      var newtoggle = toggleselectlevel.GetComponent<ToggleController>();
      newtoggle.toggle.group = togglegroup_selectloadlevel;
      newtoggle.SetToggleLabel(levelname.Substring(0, levelname.Length - 5));
      newtoggle.toggle_index = i;

      togglelist_levelselection.Add(toggleselectlevel);
      i += 1;
    }
  }

  private void ClearToggleLevelSelectionList()
  {
    for(int i = 0; i < togglelist_levelselection.Count; i++)
    {
      Destroy(togglelist_levelselection[i]);
    }
    togglelist_levelselection.Clear();
  }

}