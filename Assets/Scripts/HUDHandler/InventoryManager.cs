using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ectoCountText;
    private int ectoCount = 0;
    private TopDownCharacterController controller;
    [SerializeField] private Image hatSpriteHUD;
    private Sprite defaultHatSprite;
    ItemSO currentHat;

    private AnimatorOverrideController animOverride;
    private Animator anim;

    SlotHUDHandler[] inventorySlots;
    public bool isUnderNegotiation = false;
    private NPC_Merchant currentMerchant;
    private void Awake()
    {
        inventorySlots = FindObjectsOfType<SlotHUDHandler>();
        anim = GetComponent<Animator>();

        animOverride = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = animOverride;

        UpdateEctoCount(0);
    }
    public void RemoveOfInventory(ItemSO toRemove, int profit = 0) //remove from inventory, if profit above 0 then update the ectoplasm
    {
        foreach (SlotHUDHandler slotToAdd in inventorySlots)
        {
            if (slotToAdd.thisItem == toRemove)
            {
                slotToAdd.ReduceQuantity();
                break;
            }
        }

        if (profit > 0)
            UpdateEctoCount(ectoCount += profit);
    }
    public void AddToInventory(ItemSO toAdd, int cost = 0) //add to inventory, if cost above 0 then it'll be checked if it's possible
    {

        if (cost > 0)
        {
            if (cost > ectoCount)
                return;


            UpdateEctoCount(ectoCount -= cost);
            currentMerchant.SayBuyingLine();
        }

        foreach (SlotHUDHandler slotToAdd in inventorySlots)
        {
            if (slotToAdd.thisItem == toAdd)
            {
                slotToAdd.AddQuantity();
                break;
            }
        }

    }

    public void SlotClicked(SlotHUDHandler whichSlot)
    {
        if (!isUnderNegotiation)
        {
            if (whichSlot.thisItem.isEquipable)
            {
                EquipHat(whichSlot.thisItem);
                whichSlot.ReduceQuantity();
            }
        }
        else
        {
            RemoveOfInventory(whichSlot.thisItem, whichSlot.thisItem.itemPriceToSell);
            currentMerchant.SaySellingLine();
        }
    }

    private void EquipHat(ItemSO whichHat)
    {
        if(defaultHatSprite == null)
            defaultHatSprite = hatSpriteHUD.sprite;

        hatSpriteHUD.sprite = whichHat.itemSprite;

        animOverride["HatDown"] = whichHat.equipableClipDown;
        animOverride["HatUp"] = whichHat.equipableClipUp;
        animOverride["HatLeft"] = whichHat.equipableClipLeft;
        animOverride["HatRight"] = whichHat.equipableClipRight;

        if (controller == null)
            controller = GetComponent<TopDownCharacterController>();

        controller.currentHatLuck = whichHat.itemRarity / 2;
        currentHat = whichHat;

    }

    public void UnequipHat()
    {
        if (hatSpriteHUD.sprite != defaultHatSprite && defaultHatSprite != null)
        {
            animOverride["HatDown"] = null;
            animOverride["HatUp"] = null;
            animOverride["HatLeft"] = null;
            animOverride["HatRight"] = null;

            hatSpriteHUD.sprite = defaultHatSprite;

            controller.currentHatLuck = 1;

            AddToInventory(currentHat);
        }
    }

    public void UpdateEctoCount(int value)
    {
        ectoCount = value;
        ectoCountText.text = ectoCount.ToString("00");
    }

    public void SetToUnderNegotiation(NPC_Merchant whichMerchant = null)
    {
        if (whichMerchant != null)
        {
            isUnderNegotiation = true;
        }
        else
        {
            isUnderNegotiation = false;
        }
        currentMerchant = whichMerchant;
    }

    
}
