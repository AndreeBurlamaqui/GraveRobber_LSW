using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : MonoBehaviour, IInteractable, IDiggable
{
    [Header("Feedback related")]
    [SerializeField] private SpriteRenderer interactableSign;
    [SerializeField] private float signAnimDuration;
    [SerializeField] private GameObject dustFX;

    [Header("Dig related")]
    [SerializeField] private GameObject prefabDrop;
    [SerializeField] Transform[] itemPlaces = new Transform[3];
    [SerializeField] private DropablesItem items;
    [SerializeField] private SpriteRenderer diggablePart;
    [SerializeField] private Sprite[] stages = new Sprite[4];
    private int stageCount = 4, quantityDropped = 0;
    [SerializeField] private bool hasShowedItems = false;
    private float currentRarityQuantity = 1f;
    [SerializeField] private float multiplierQuantity = 1.2f;

    public bool hasBeenDug { get; set; }
    public int hatLuck { get; set; }

    public void OnInteract()
    {

        if (stageCount > 0)
        {
            stageCount--;

            diggablePart.sprite = stages[stageCount];
            dustFX.SetActive(true);
        }
        else if(!hasShowedItems)
        {
            if (hatLuck < 1)
                hatLuck = 1;

            hasShowedItems = true;
            hasBeenDug = true;
            DropItems();
            dustFX.SetActive(true);
        }

    }

    private void DropItems()
    {

        if (quantityDropped < 3)
        {
            int rarityTypeDrop = WhichRarityToDrop();

            if (rarityTypeDrop > 0)
            {
                List<ItemSO> howManyOfThisType = new List<ItemSO>();

                foreach (ItemSO itemToDrop in items.itensToFind)
                {
                    if (itemToDrop.itemRarity == rarityTypeDrop)
                        howManyOfThisType.Add(itemToDrop);
                }

                DropHolder newDrop = Instantiate(prefabDrop, itemPlaces[quantityDropped].position, Quaternion.identity).GetComponent<DropHolder>();

                newDrop.thisDropItemSO = howManyOfThisType[Random.Range(0, howManyOfThisType.Count)];
                newDrop.UpdateSprite();
            }

            quantityDropped++;
            DropItems();
        }
        else
        {
            StartCoroutine(GetComponent<IInteractable>().HideInteractable());
        }
        
    }

    private int WhichRarityToDrop()
    {
        //Set which rarity types will be chosen and how many items will be dropped

        float rarityValue = Random.value; //If this value doesn't become bigger than 0.25 then there'll be no item in the tombstone

        

        if (((rarityValue / currentRarityQuantity) * hatLuck) > 0.85f) // supernatural = 15% (1 - 0.85 = 0.15)
        {
            currentRarityQuantity *= multiplierQuantity;
            return 3;
        }
        else if (((rarityValue / currentRarityQuantity) * hatLuck) > 0.65f) // rare = 35% (1 - 0.65 = 0.35)
        {
            currentRarityQuantity *= multiplierQuantity;
            return 2;
        } 
        else if (((rarityValue / currentRarityQuantity) * hatLuck) > 0.25f) // common = 75% (1 - 0.25 = 0.25)
        {
            return 1;
        }

        return 0;
    }

    public IEnumerator ShowInteractable()
    {
        if (quantityDropped < 3)
        {
            float journey = 0;
            Color newAlpha = interactableSign.color;
            while (journey <= signAnimDuration)
            {
                journey += Time.deltaTime;
                float percent = journey / signAnimDuration;

                newAlpha.a = percent;
                interactableSign.color = newAlpha;

                yield return null;
            }
        }
    }

    public IEnumerator HideInteractable()
    {
        if (interactableSign.color.a > 0)
        {
            float journey = 1;
            Color newAlpha = interactableSign.color;
            while (journey >= 0)
            {
                journey -= Time.deltaTime;

                newAlpha.a = Mathf.Clamp01(journey);
                interactableSign.color = newAlpha;

                yield return null;
            }
        }
    }
}
