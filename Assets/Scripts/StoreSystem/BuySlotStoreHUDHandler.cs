using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BuySlotStoreHUDHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ItemSO thisItem;
    [SerializeField] TMPro.TextMeshProUGUI priceText;
    [SerializeField] UnityEngine.UI.Image itemImage;
    InventoryManager iM;
    NPC_Merchant thisNPC;
    private void OnEnable()
    {
        priceText.text = thisItem.itemPriceToBuy.ToString("00");
        itemImage.sprite = thisItem.itemSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (iM == null)
            iM = FindObjectOfType<InventoryManager>();

        iM.AddToInventory(thisItem, thisItem.itemPriceToBuy);

    }
}
