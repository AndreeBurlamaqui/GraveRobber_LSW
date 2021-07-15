using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ItemSO")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;

    [Tooltip("1 - Common (75%) ||| 2 - Rare (35%) ||| 3 - Supernatural (15%)")]
    public int itemRarity;

    public bool isEquipable = false;

    [Tooltip("If isEquipable, then the price is to be bought. If not, is to be selled")]
    public int price;
}
