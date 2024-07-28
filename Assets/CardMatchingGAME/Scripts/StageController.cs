using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.AI;

public class StageController : MonoBehaviour
{
  public static StageController instance;

  public List<Card> cards_stage;

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
  private int card_scoretopasslevel = 5;

  public UnityEvent onPassLevelEvent;
  public UnityEvent<int> onUpdateMatchingScoreEvent;
  private int card_matchingScore = 0;
  public int MatchingScore
  {
    get { return card_matchingScore; }
    set
    {
      card_matchingScore = value;
      onUpdateMatchingScoreEvent?.Invoke(card_matchingScore);
      if(card_matchingScore == card_scoretopasslevel)
      {
        bool isGameEnding = LevelManager.instance.CheckingGameEnding();
        if (!isGameEnding)
        {
          onPassLevelEvent?.Invoke();
        }
        else
        {
          LevelManager.instance.onGameEndingEvent?.Invoke();
        }
      }
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

  [SerializeField]
  private AudioSource audioSourceEffect;
  [SerializeField]
  private AudioClip matching_audio;
  [SerializeField]
  private AudioClip mismatching_audio;

  public List<int> debug_cardsTypePool = new List<int>();

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
    //LevelManager.instance.onGameEndingEvent.AddListener(PlayAudioGameEnding);
  }

  void Update()
  {
    /*if (Input.GetKeyDown(KeyCode.S))
    {
      CreateCardPoolStage(card_spawnRow, card_spawnColumn);
    }*/
  }

  public void StartStage(LevelDataScriptableObject leveldata)
  {
    MatchingScore = 0;
    MatchingTurn = 0;
    cardsMatching.Clear();
    card_matchingcount = leveldata.level_cardmatchingcount;
    card_scoretopasslevel = leveldata.level_scoretopasslevel;
    stage_delaycardFlip = leveldata.level_delaycardflip;

    CreateCardPoolStage(leveldata.level_cardspawnRow, leveldata.level_cardspawnColumn);
  }

  public void SetupStageData(int cardmatchingcount)
  {
    card_matchingcount = cardmatchingcount;
  }

  List<int> CreateRandomCardsPool(int cardTypeNumbers)
  {
    List<int> cardsTypePool = new List<int>();

    List<CardDataScriptableObject> randomCards = new List<CardDataScriptableObject>();
    
    if(cardSpawner.cardDatas.Count >= cardTypeNumbers)
    {
      randomCards = cardSpawner.cardDatas.OrderBy(x => Guid.NewGuid()).Take(cardTypeNumbers).ToList();
    }
    else if(cardSpawner.cardDatas.Count < cardTypeNumbers)
    {
      randomCards = cardSpawner.cardDatas.OrderBy(x => Guid.NewGuid()).Take(cardSpawner.cardDatas.Count).ToList();

      int additionalCardsType = cardTypeNumbers - cardSpawner.cardDatas.Count;

      var randomAdditionalCards = cardSpawner.cardDatas.OrderBy(x => Guid.NewGuid()).Take(additionalCardsType).ToList();

      randomCards.AddRange(randomAdditionalCards);
    }

    foreach (var card in randomCards)
    {
      for (int j = 0; j < card_matchingcount; j++)
      {
        cardsTypePool.Add(card.card_typeIndex);
      }
    }
    cardsTypePool = cardsTypePool.OrderBy(x => Guid.NewGuid()).Take(cardsTypePool.Count).ToList();

    return cardsTypePool;
  }

  public void CreateCardPoolStage(int cardRowNumber, int cardColumnNumber)
  {
    ClearCardPoolStage();
    bool isCardNumbersRequireAdditional = (cardRowNumber * cardColumnNumber) % card_matchingcount == 0;

    int cardNumbers = cardRowNumber * cardColumnNumber;
    List<int> cardsTypePool = new List<int>();
    
    if (!isCardNumbersRequireAdditional)
    {
      cardNumbers -= 1;
      int randomAdditionCardType = cardSpawner.cardDatas.OrderBy(x => Guid.NewGuid()).FirstOrDefault().card_typeIndex;
      //Debug.Log("Random Additional CardType: " + randomAdditionCardType);
      cardsTypePool.Add(randomAdditionCardType);
      debug_cardsTypePool.Add(randomAdditionCardType);
    }
    int cardTypeNumbers = cardNumbers / card_matchingcount;
    //Debug.Log("Card Type To Random: " + cardTypeNumbers);

    var randomcardsPool = CreateRandomCardsPool(cardTypeNumbers);
    //Debug.Log("Random Cards Pool Count: " + randomcardsPool.Count);

    debug_cardsTypePool.AddRange(randomcardsPool);
    cardsTypePool.AddRange(randomcardsPool);

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

        if(cardsTypePool.Count > card_index)
        {
          SetupNewCardData(newcard, card_index, cardsTypePool[card_index]);
          cards_stage.Add(newcard);
          card_index++;
        }
      }
    }
  }
  void SetupNewCardData(Card card, int cardIndex, int cardTypeIndex)
  {
    //Debug.Log("SetupNewCard Index: " + cardIndex + ", Type: " + cardTypeIndex);
    card.SetupCardDataAndDisplay(cardSpawner.cardDatas.Find(x => x.card_typeIndex == cardTypeIndex));
    card.SetCardIndex(cardIndex);
    card.CallFlipCardAsync(false, stage_delaycardFlip);
  }

  public void StartLoadedStage(LevelDataScriptableObject leveldata, LevelSavedData savedData)
  {
    MatchingScore = savedData.level_matchingScore;
    MatchingTurn = savedData.level_matchingTurn;
    cardsMatching.Clear();
    card_matchingcount = leveldata.level_cardmatchingcount;
    card_scoretopasslevel = leveldata.level_scoretopasslevel;
    stage_delaycardFlip = leveldata.level_delaycardflip;

    ClearCardPoolStage();

    for (int i = 0; i < savedData.card_savedDatas.Count; i++)
    {
      var newcard = cardSpawner.GetCard();
      var cardSavedData = savedData.card_savedDatas[i];

      newcard.transform.localPosition = cardSavedData.card_spawnPosition;
      SetupNewCardData(newcard, cardSavedData.card_spawnIndex, cardSavedData.card_typeIndex);
      cards_stage.Add(newcard);
    }
  }

  public void InteractCardFlip(GameObject cardObject)
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
    audioSourceEffect.clip = mismatching_audio;
    audioSourceEffect.Play();
    foreach (var card in cards)
    {
      card.CallFlipCardAsync(false, 0);
    }
  }
  void OnMatchingCards(List<Card> cards)
  {
    MatchingScore++;
    audioSourceEffect.clip = matching_audio;
    audioSourceEffect.Play();
    foreach (var card in cards)
    {
      cards_stage.Remove(card);
      card.cardPool.Release(card);
    }
  }

  public List<CardSavedData> CreateStageCardSavedDatas()
  {
    List<CardSavedData> newCardSavedDatas = new List<CardSavedData>();

    foreach(Card card in cards_stage)
    {
      CardSavedData newsavedData = new CardSavedData();
      newsavedData.card_spawnIndex = card.GetCardIndex();
      newsavedData.card_typeIndex = card.card_typeIndex;
      newsavedData.card_spawnPosition = card.transform.localPosition;
      newCardSavedDatas.Add(newsavedData);
    }

    return newCardSavedDatas;
  }

}