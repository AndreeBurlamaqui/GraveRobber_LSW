using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHolder : MonoBehaviour
{
    public ItemSO thisDropItemSO;

    public void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = thisDropItemSO.itemSprite;
    }
}
