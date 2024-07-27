using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
  public string card_name;
  public int card_typeIndex;
  public MeshRenderer card_renderer;

  private int cart_index;

  public IObjectPool<Card> cardPool;

  /*[SerializeField]
  private CardDataScriptableObject cardData;
  */
  public bool isCardfilpped = false;
  public bool isFlipping = false;
  [SerializeField]
  private float card_flipduration = 0.5f;
  [SerializeField]
  private Ease setflipanimationEase;


  List<Task> tasks = new List<Task>();

  void Start()
  {

  }
  /*void Update()
  {
    if (Input.GetKeyDown(KeyCode.F))
    {
      CallFlipCardAsync(false, 0);
      //CallFlipCardAsync(true, 0);
    }
    if (Input.GetKeyDown(KeyCode.D))
    {
      CallFlipCardAsync(true, 0);
    }
  }*/

  public void SetupCardDataAndDisplay(CardDataScriptableObject data)
  {
    card_name = data.card_name;
    card_typeIndex = data.card_typeIndex;

    if (card_renderer != null)
    {
      card_renderer.material = data.card_displayMaterial;
    }
  }

  public async void CallFlipCardAsync(bool isFlipfront, int delayStartmillisec, Action afterFlipaction = null)
  {
    await Task.Delay(delayStartmillisec);

    if (isFlipfront)
    {
      await FlipToFrontAsync();
    }
    else
    {
      await FlipToBackAsync();
    }
    afterFlipaction?.Invoke();
  }

  public async Task FlipToFrontAsync()
  {
    if (isFlipping) return;

    isFlipping = true;

    await RotateCard(0);

    isFlipping = false;
    isCardfilpped = false;
  }
  public async Task FlipToBackAsync()
  {
    if (isFlipping) return;

    isFlipping = true;

    await RotateCard(180);

    isFlipping = false;
    isCardfilpped = true;
  }

  private Task RotateCard(float angle)
  {
    var tcs = new TaskCompletionSource<bool>();

    transform.DOLocalRotate(new Vector3(0f, 0f, angle), card_flipduration).SetEase(setflipanimationEase).OnComplete(() => tcs.SetResult(true));

    return tcs.Task;
  }

  public void ResetFlipping()
  {
    isCardfilpped = false;
  }

  public void SetCardIndex(int index)
  {
    cart_index = index;
  }
  public int GetCardIndex()
  {
    return cart_index;
  }
}