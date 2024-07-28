using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
  public Toggle toggle;
  public Text text_label;
  public int toggle_index;

  private void Awake()
  {
    toggle.onValueChanged.AddListener(SetLevelLoading);
  }

  public void SetToggleLabel(string label)
  {
    text_label.text = label;
  }

  private void SetLevelLoading(bool isLoadingLevelIndex)
  {
    if (isLoadingLevelIndex)
    {
      LevelManager.instance.SetTargetLoadingLevel(toggle_index);
    }
  }
}
