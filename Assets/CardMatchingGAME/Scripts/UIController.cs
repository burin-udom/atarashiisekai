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
    int i = 0;
    foreach(string levelname in levelsname)
    {
      var toggleselectlevel = Instantiate(toggle_selectloadlevel, transform_selectloadlevelparent);

      var newtoggle = toggleselectlevel.GetComponent<ToggleController>();
      newtoggle.SetToggleLabel(levelname.Substring(0, levelname.Length - 5));
      newtoggle.toggle_index = i;

      i += 1;
    }
  }

}