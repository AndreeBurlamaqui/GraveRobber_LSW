using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Merchant Line Dialogues", menuName = "Merchant Line Dialogues")]
public class MerchantLineDialoguesSO : ScriptableObject
{
    public string npcName;

    [TextArea]
    public string[] greetingsLines;

    [TextArea]
    public string[] buyingLines;

    [TextArea]
    public string[] sellingLines;
}
