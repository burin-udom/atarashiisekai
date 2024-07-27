using DG.Tweening;
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

  private bool isCardfilpped = false;
  [SerializeField]
  private float card_flipduration = 0.5f;
  [SerializeField]
  private Ease setflipanimationEase;

  void Start()
  {

  }
  void Update()
  {
    if(Input.GetKeyDown(KeyCode.C))
    {
      SetupCardDataAndDisplay(cardData);
    }
    if (Input.GetKeyDown(KeyCode.F))
    {
      FlipCard();
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

  public void FlipCard()
  {
    if(isCardfilpped)
    {
      var flipping = transform.DOLocalRotate(new Vector3(0f, 0f, 0f), card_flipduration).SetEase(setflipanimationEase).OnComplete(() =>
      {
        isCardfilpped = false;
      });
    }
    else
    {
      var flipping = transform.DOLocalRotate(new Vector3(0f, 0f, 180f), card_flipduration).SetEase(setflipanimationEase).OnComplete(() =>
      {
        isCardfilpped = true;
      });
    }
  }
}