using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Card : MonoBehaviour
{
  public string card_name;
  public int card_typeIndex;
  public MeshRenderer card_renderer;

  [SerializeField]
  private CardDataScriptableObject cardData;

  void Start()
  {

  }
  void Update()
  {
    if(Input.GetKeyDown(KeyCode.C))
    {
      SetupCardDataAndDisplay(cardData);
    }

  }

  public void SetupCardDataAndDisplay(CardDataScriptableObject data)
  {
    card_name = data.card_name;
    card_typeIndex = data.card_typeIndex;

    if(card_renderer != null)
    {
      card_renderer.material = data.card_displayMaterial;
    }
  }
}