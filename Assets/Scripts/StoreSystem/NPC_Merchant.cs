using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC_Merchant : MonoBehaviour, IInteractable
{
    [SerializeField] MerchantLineDialoguesSO npcLines;
    [SerializeField] SpriteRenderer interactableSign;
    [SerializeField] private float signAnimDuration, textTypeDelay;
    [SerializeField] private GameObject interactCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText, nameText;
    bool isNegotiating = false, isTalking = false;
    private InventoryManager iM;
    public IEnumerator HideInteractable()
    {
        if(isNegotiating)
            ResetStore();


        
            float journey = 1;
            Color newAlpha = interactableSign.color;
            while (journey >= 0)
            {
                journey -= Time.deltaTime * 2;

                newAlpha.a = Mathf.Clamp01(journey);
                interactableSign.color = newAlpha;

                yield return null;
            }

        
    }

    public void OnInteract()
    {
        if (!isNegotiating)
        {
            interactCanvas.SetActive(true);
            isNegotiating = true;
            nameText.text = npcLines.npcName;
            dialogueText.text = npcLines.greetingsLines[Random.Range(0, npcLines.greetingsLines.Length)];
            StartCoroutine(TypeDialogue());

            if (iM == null)
                iM = FindObjectOfType<InventoryManager>();

            iM.SetToUnderNegotiation(this);
        }
    }

    private void ResetStore()
    {
        isNegotiating = false;
        interactCanvas.SetActive(false);
        iM.SetToUnderNegotiation();

    }

    IEnumerator TypeDialogue()
    {
        isTalking = true;
        for (int x = 0; x < dialogueText.text.Length; x++)
        {
            dialogueText.maxVisibleCharacters = x + 1;
            dialogueText.ForceMeshUpdate();

            yield return new WaitForSeconds(textTypeDelay);

        }
        isTalking = false;
    }

    public IEnumerator ShowInteractable()
    {
        if (!isNegotiating)
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

    /// <summary>
    /// When the player is buying from the Merchant
    /// </summary>
    public void SayBuyingLine()
    {
        if (!isTalking)
        {
            dialogueText.text = npcLines.buyingLines[Random.Range(0, npcLines.buyingLines.Length)];
            StartCoroutine(TypeDialogue());
        }
    }

    /// <summary>
    /// When the player is selling for the merchant
    /// </summary>
    public void SaySellingLine()
    {
        if (!isTalking)
        {
            dialogueText.text = npcLines.sellingLines[Random.Range(0, npcLines.sellingLines.Length)];
            StartCoroutine(TypeDialogue());
        }
    }
}
