using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DropHolder : MonoBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler
{
    public ItemSO thisDropItemSO;
    private TextMeshProUGUI tooltip;

    [SerializeField] private Material outlineMaterial;
    private Material normalMaterial;


    private void Start() => UpdateSprite();
    public IEnumerator HideInteractable()
    {
        if (TryGetComponent(out Renderer render))
        {
            render.material = normalMaterial;

            yield return null;
        }
    }

    public void OnInteract()
    {
        //SlotHUDHandler[] checkSlots = FindObjectsOfType<SlotHUDHandler>();

        //foreach(SlotHUDHandler toAdd in checkSlots)
        //{
        //    if(toAdd.thisItem == thisDropItemSO)
        //    {
        //        toAdd.quantity = 1;
        //        Destroy(gameObject);
        //        break;
        //    }
        //}

        FindObjectOfType<InventoryManager>().AddToInventory(thisDropItemSO);
        Destroy(gameObject);

    }

    public IEnumerator ShowInteractable()
    {
        if (TryGetComponent(out Renderer render))
        {
            render.material = outlineMaterial;

            yield return null;
        }
    }

    public void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = thisDropItemSO.itemSprite;

        if (tooltip == null)
            tooltip = GetComponentInChildren<TextMeshProUGUI>();

        tooltip.GetComponentInParent<Canvas>().worldCamera = Camera.main;
        tooltip.text = thisDropItemSO.itemName;
        tooltip.gameObject.SetActive(false);

        if (TryGetComponent(out Renderer render))
        {
            normalMaterial = render.material;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
