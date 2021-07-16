using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class SlotHUDHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemSO thisItem;

    public int quantity = 0;

    [SerializeField] private GameObject priceTooltip;
    private TextMeshProUGUI quantityText;
    private InventoryManager iM;

    public void AddQuantity()
    {
        quantity++;
        UpdateQuantity();
    }
    public void ReduceQuantity()
    {
        quantity--;
        UpdateQuantity();
    }
    private void UpdateQuantity()
    {
        if (quantityText == null)
            quantityText = GetComponentInChildren<TextMeshProUGUI>();

        quantityText.text = quantity.ToString("00");
    }

    private void Start()
    {
        UpdateQuantity();
        iM = FindObjectOfType<InventoryManager>();
        priceTooltip.SetActive(false);
        priceTooltip.GetComponentInChildren<TextMeshProUGUI>().text = thisItem.itemPriceToSell.ToString("00");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (quantity > 0)
            iM.SlotClicked(this);


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (iM.isUnderNegotiation) //show price to sell
        {
            priceTooltip.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (priceTooltip.activeInHierarchy) //show price to sell
        {
            priceTooltip.SetActive(false);

        }
    }
}
