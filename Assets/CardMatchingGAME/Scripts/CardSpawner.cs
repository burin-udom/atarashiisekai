using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CardSpawner : MonoBehaviour
{
  public static CardSpawner instance;

  [SerializeField]
  private GameObject card_prefab;
  [SerializeField]
  private Transform card_spawnParent;

  private IObjectPool<Card> card_Objectpool;

  [SerializeField]
  private bool collectionCheck = true;
  [SerializeField]
  private int defaultCapacity = 20;
  [SerializeField]
  private int maxSize = 100;

  public List<CardDataScriptableObject> cardDatas = new List<CardDataScriptableObject>();

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
    card_Objectpool = new ObjectPool<Card>(OnCreateCard, OnGetCardFromPool, OnReleaseCardFromPool, OnDestroyCardObject, collectionCheck, defaultCapacity, maxSize);
  }

  /*private void Update()
  {
    if(Input.GetKeyDown(KeyCode.C))
    {
      card_Objectpool.Get();
    }
  }*/

  private Card OnCreateCard()
  {
    Card newCard = Instantiate(card_prefab, card_spawnParent).GetComponent<Card>();

    newCard.cardPool = card_Objectpool;
    SetupNewCardData(newCard);

    return newCard;
  }
  private void OnGetCardFromPool(Card cardObject)
  {
    /*float posX = Random.Range(0f, 7.5f);
    float posZ = Random.Range(-4.25f, 0.25f);
    SetupNewCardData(cardObject);

    float posX = Random.Range(0f, 7.5f);
    float posZ = Random.Range(-4.25f, 0.25f);*/

    cardObject.transform.localPosition = card_prefab.transform.localPosition;
    cardObject.gameObject.SetActive(true);
  }
  private void OnReleaseCardFromPool(Card cardObject)
  {
    cardObject.gameObject.SetActive(false);
  }
  private void OnDestroyCardObject(Card cardObject)
  {
    Destroy(cardObject.gameObject);
  }

  void SetupNewCardData(Card card)
  {
    int randomCardType = Random.Range(0, cardDatas.Count);
    card.SetupCardDataAndDisplay(cardDatas[randomCardType]);
  }

  public Card GetCard()
  {
    Card card = card_Objectpool.Get();

    return card;
  }

}
