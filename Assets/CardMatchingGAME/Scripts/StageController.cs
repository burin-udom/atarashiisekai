using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
  public static StageController instance;

  public List<Card> cards_stage;

  [SerializeField]
  private int card_spawnRow = 1;
  [SerializeField]
  private int card_spawnColumn = 1;

  [SerializeField]
  private int stage_delaycardFlip = 1000;

  [SerializeField]
  private float spacing_posX = 1.5f;
  [SerializeField]
  private float spacing_posZ = -1.5f;

  CardSpawner cardSpawner;

  private List<Card> cardsMatching = new List<Card>();
  [SerializeField]
  private int card_matchingcount = 2;

  public UnityEvent<int> onUpdateMatchingScoreEvent;
  private int card_matchingScore = 0;
  public int MatchingScore
  {
    get { return card_matchingScore; }
    set
    {
      card_matchingScore = value;
      onUpdateMatchingScoreEvent?.Invoke(card_matchingScore);
    }
  }

  public UnityEvent<int> onUpdateMatchingTurnEvent;
  private int card_matchingTurn = 0;
  public int MatchingTurn
  {
    get { return card_matchingTurn; }
    set
    {
      card_matchingTurn = value;
      onUpdateMatchingTurnEvent?.Invoke(card_matchingTurn);
    }
  }

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
    cardSpawner = CardSpawner.instance;
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.S))
    {
      CreateCardPoolStage(card_spawnRow, card_spawnColumn);
    }
  }

  public void CreateCardPoolStage(int cardRowNumber, int cardColumnNumber)
  {
    ClearCardPoolStage();
    int card_index = 0;
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
        newcard.SetCardIndex(card_index);
        newcard.CallFlipCardAsync(false, stage_delaycardFlip);
        cards_stage.Add(newcard);
        card_index++;
      }
    }
  }

  public void CallCardFlip(GameObject cardObject)
  {
    var card = cardObject.GetComponent<Card>();
    if (card != null)
    {
      if (card.isCardfilpped && !card.isFlipping)
      {
        cardsMatching.Add(card); 
        if (cardsMatching.Count == card_matchingcount)
        {
          MatchingTurn++;
          List<Card> checkingCardMatch = new List<Card>(cardsMatching);
          card.CallFlipCardAsync(true, 0, () => {

            CheckCardMatching(checkingCardMatch);

          });
          cardsMatching.Clear();
        }
        else
        {
          card.CallFlipCardAsync(true, 0);
        }
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
    if (cards_stage.Count > 0)
    {
      foreach (var card in cards_stage)
      {
        card.cardPool.Release(card);
      }
      cards_stage.Clear();
    }
  }

  void CheckCardMatching(List<Card> cards)
  {
    //Debug.Log("Checking Card Matching : " + cards.Count);
    bool isCardMatching = false;
    if(cards.Count == card_matchingcount)
    {
      //MatchingTurn++;
      var firstFlipFrontCard = cards[0];

      int matchingScore = 1;
      for(int i = 1; i < cards.Count; i++)
      {
        if (cards[i].card_typeIndex == firstFlipFrontCard.card_typeIndex)
        {
          matchingScore += 1;
        }
      }

      if(matchingScore == cards.Count)
      {
        isCardMatching = true;
      }

      if (isCardMatching)
      {
        OnMatchingCards(cards);
      }
      else
      {
        OnUnMatchingCard(cards);
      }
    }
  }
  void OnUnMatchingCard(List<Card> cards)
  {
    foreach (var card in cards)
    {
      card.CallFlipCardAsync(false, 0);
    }
  }
  void OnMatchingCards(List<Card> cards)
  {
    MatchingScore++;
    foreach (var card in cards)
    {
      cards_stage.Remove(card);
      card.cardPool.Release(card);
    }
  }

}