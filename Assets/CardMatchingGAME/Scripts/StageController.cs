using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
  public List<Card> cards;

  [SerializeField]
  private int card_spawnRow = 1;
  [SerializeField]
  private int card_spawnColumn = 1;

  [SerializeField]
  private float spacing_posX = 1.5f;
  [SerializeField]
  private float spacing_posZ = -1.5f;

  CardSpawner cardSpawner;

  void Start()
  {
    cardSpawner = CardSpawner.instance;

  }

  /*void Update()
  {
    if (Input.GetKeyDown(KeyCode.S))
    {
      CreateCardPoolStage(card_spawnRow, card_spawnColumn);
    }
    if (Input.GetKeyDown(KeyCode.C))
    {
      ClearCardPoolStage();
    }
  }*/

  public void CreateCardPoolStage(int cardRowNumber, int cardColumnNumber)
  {
    ClearCardPoolStage();
    for (int z = 0; z < cardRowNumber; z++)
    {
      float posZ = z * spacing_posZ;
      for (int x = 0; x < cardColumnNumber; x++)
      {
        float posX = x * spacing_posX;
        var newcard = cardSpawner.GetCard();

        newcard.transform.localPosition = new Vector3(
          newcard.transform.localPosition.x + posX, 
          newcard.transform.localPosition.y,
          newcard.transform.localPosition.z + posZ);

        SetupNewCardData(newcard);
        cards.Add(newcard);
      }
    }
  }

  void SetupNewCardData(Card card)
  {
    int randomCardType = Random.Range(0, cardSpawner.cardDatas.Count);
    card.SetupCardDataAndDisplay(cardSpawner.cardDatas[randomCardType]);
  }

  public void ClearCardPoolStage()
  {
    if(cards.Count > 0)
    {
      foreach (var card in cards)
      {
        card.cardPool.Release(card);
      }
      cards.Clear();
    }
  }

}