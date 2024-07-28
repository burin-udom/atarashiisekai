using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
  public Text text_matchingscoreDisplay;
  public Text text_matchingturnDisplay;

  void Start()
  {

  }

  void Update()
  {

  }

  public void DisplayMatchingScore(int score)
  {
    text_matchingscoreDisplay.text = score.ToString();
  }
  public void DisplayMatchingTurn(int turn)
  {
    text_matchingturnDisplay.text = turn.ToString();
  }

}