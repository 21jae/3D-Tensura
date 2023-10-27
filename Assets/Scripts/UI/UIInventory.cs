using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public static bool INVENTORY_ACTIVATED = false;

    [SerializeField] private GameObject InventoryBase;  //UI_InventoryBase
    [SerializeField] private GameObject SlotsParent;    //UI_Grid Setting

    private UISlot[] slots;

    private void Start()
    {
        slots = SlotsParent.GetComponentsInChildren<UISlot>();
    }

    private void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            INVENTORY_ACTIVATED = !INVENTORY_ACTIVATED;

            if (INVENTORY_ACTIVATED)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        //장비 아이템이 아닐경우엔 갯수추가,
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)   //슬롯안에 아이템이 있다면
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)   //슬롯안이 비어있따면
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
