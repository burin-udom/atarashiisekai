using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardDataScriptableObject : ScriptableObject
{
  public string card_name;
  public int card_typeIndex;
  public Sprite card_displaySprite;
}
