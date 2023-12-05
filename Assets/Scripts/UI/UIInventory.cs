using System;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public static bool INVENTORY_ACTIVATED = false;
    public static UIInventory instance;

    [SerializeField] private GameObject inventoryOpen;                  //On
    [SerializeField] private GameObject inventoryOff;                   //Off
    [SerializeField] private GameObject skillOff;                       //On
    [SerializeField] private GameObject InventoryBase;                  //UI_InventoryBase
    [SerializeField] private GameObject SlotsParent;                    //UI_Grid Setting
    [SerializeField] private UICharacterEquipment characterEquipment;   //UI_Equip 

    public event Action<Item> onItemAcquired;

    private UISlot[] slots;

    private void Awake()
    {
        instance = this;
        inventoryOff.gameObject.SetActive(false);
        slots = SlotsParent.GetComponentsInChildren<UISlot>();
    }

    private void Update()
    {
        TryOpenInventory();
    }

    public void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            INVENTORY_ACTIVATED = !INVENTORY_ACTIVATED;

            if (INVENTORY_ACTIVATED)
            {
                OpenInventory();
                SoundManager.Instance.OpenSound();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    public void OpenInventory()
    {
        InventoryBase.SetActive(true);
        inventoryOpen.gameObject.SetActive(false);
        inventoryOff.gameObject.SetActive(true);
        skillOff.gameObject.SetActive(false);

    }

    public void CloseInventory()
    {
        InventoryBase.SetActive(false);
        inventoryOff.gameObject.SetActive(false);
        inventoryOpen.gameObject.SetActive(true);
        skillOff.gameObject.SetActive(true);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        //��� �������� �ƴҰ�쿣 �����߰�,
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)   //���Ծȿ� �������� �ִٸ�
                    {
                        slots[i].SetSlotCount(_count);
                        onItemAcquired?.Invoke(_item);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)   //���Ծ��� ����ֵ���
            {
                slots[i].AddItem(_item, _count);
                onItemAcquired?.Invoke(_item);
                return;
            }
        }
    }

    public void TryEquipment(UISlot slot)
    {
        if (slot.item.itemType == Item.ItemType.Equipment)
        {
            characterEquipment.EquipmentItem(slot.item);
            slot.ClearSlot();
        }
    }
}
