using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dropables", menuName = "Dropables")]
public class DropablesItem : ScriptableObject
{
    public List<ItemSO> itensToFind;
}
