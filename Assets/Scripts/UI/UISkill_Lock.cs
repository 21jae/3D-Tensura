using DialogueEditor;
using System;
using UnityEngine;

public class UISkill_Lock : MonoBehaviour
{
    //[SerializeField] private NPCConversation myConversation;
    //public GameObject unlockImages;
    public GameObject skill01;

    public bool isDeterioration = true;

    //private void Start()
    //{
    //    unlockImages.SetActive(true);
    //    UIInventory.instance.onItemAcquired += CheckForItem;
    //}

    //private void OnDestroy()
    //{
    //    UIInventory.instance.onItemAcquired -= CheckForItem;
    //}

    //private void CheckForItem(Item item)
    //{
    //    if (item.itemName == "IzawaShizue")
    //    {
    //        isDeterioration = true;
    //    }
    //}

    public void UnLockSkills()
    {
        if (isDeterioration == true)
        {
            //unlockImages.SetActive(false);
            skill01.SetActive(true);
        }
    }
}
