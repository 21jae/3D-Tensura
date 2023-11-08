using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot_Equipment : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    public Item item;
    [SerializeField] private Image itemImage;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite emptySlotSprite;
    [SerializeField] private Sprite filledSlotSprite;


    public void SetItem(Item newItem)
    {
        item = newItem;
        UpdateItemImage();
    }

    private void UpdateItemImage()
    {
        if (item != null)
        {
            itemImage.sprite = item.itemImage;
            itemImage.color = Color.white;
            backgroundImage.sprite = filledSlotSprite;

        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            backgroundImage.sprite = emptySlotSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                //��� ����, �� ���� �����ֱ� ��
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        UISlot_Equipment draggedSlot = eventData.pointerDrag.GetComponent<UISlot_Equipment>();
        if (draggedSlot != null && draggedSlot.item != null)
        {
            if (CanEquip(draggedSlot.item))
            {
                SetItem(draggedSlot.item);
                draggedSlot.ClearSlot();
            }
        }
    }

    private bool CanEquip(Item itemToEquip)
    {
        //����Ҽ��ִ� ���� ���� ����
        return true;
    }
    private void ClearSlot()
    {
        SetItem(null);
    }
}
