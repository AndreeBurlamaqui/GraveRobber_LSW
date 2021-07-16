using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[CreateAssetMenu(fileName = "Item", menuName = "ItemSO")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public int itemPriceToBuy;
    public int itemPriceToSell;


    [Tooltip("1 - Common (75%) ||| 2 - Rare (35%) ||| 3 - Supernatural (15%)")]
    public int itemRarity;

    public bool isEquipable = false;

    [ConditionalField(nameof(isEquipable))] public AnimationClip equipableClipUp;
    [ConditionalField(nameof(isEquipable))] public AnimationClip equipableClipDown;
    [ConditionalField(nameof(isEquipable))] public AnimationClip equipableClipLeft;
    [ConditionalField(nameof(isEquipable))] public AnimationClip equipableClipRight;


}
