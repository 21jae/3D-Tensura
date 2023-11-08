using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public static bool INVENTORY_ACTIVATED = false;
    public static UIInventory instance;

    [SerializeField] private GameObject inventoryOpen;                  //On
    [SerializeField] private GameObject inventoryOff;                   //Off
    [SerializeField] private GameObject InventoryBase;                  //UI_InventoryBase
    [SerializeField] private GameObject SlotsParent;                    //UI_Grid Setting
    [SerializeField] private UICharacterEquipment characterEquipment;   //UI_Equip 

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
    }

    public void CloseInventory()
    {
        InventoryBase.SetActive(false);
        inventoryOff.gameObject.SetActive(false);
        inventoryOpen.gameObject.SetActive(true);
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

    public void TryEquipment(UISlot slot)
    {
        if (slot.item.itemType == Item.ItemType.Equipment)
        {
            characterEquipment.EquipmentItem(slot.item);
            slot.ClearSlot();
        }
    }
}
