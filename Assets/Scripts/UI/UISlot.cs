using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler ,IDropHandler 
{
    [Header("Background Images")]
    [SerializeField] private Image itemBackgroundImage; //������ ����̹���
    [SerializeField] private Sprite defaultBg;  //�⺻���
    [SerializeField] private Sprite bg1;    //equipment ���
    [SerializeField] private Sprite bg2;    //Used ���
    [SerializeField] private Sprite bg3;    //Ingredient ���
    [SerializeField] private Sprite bg4;    //ETC���
    [SerializeField] private UISlotToolTip slotTooltip;

    public Item item;
    public int itemCount;
    public Image itemImage;

    [SerializeField] private GameObject CountImage;
    [SerializeField] private Text text_Count;


    //�̹��� ���� ����
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //������ ȹ��
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            CountImage.SetActive(false);
        }

        UpdateBackgroundImage();

        SetColor(1);    //������ �����ֱ�
    }


    //������ ���� ����
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    private void UpdateBackgroundImage()
    {
        if (item == null)
        {
            itemBackgroundImage.sprite = null;
            return;
        }

        switch (item.itemType)
        {
            case Item.ItemType.Equipment:
                itemBackgroundImage.sprite = bg1;
                break;
            case Item.ItemType.Used:
                itemBackgroundImage.sprite = bg2;
                break;
            case Item.ItemType.Ingredient:
                itemBackgroundImage.sprite = bg3;
                break;
            case Item.ItemType.ETC:
                itemBackgroundImage.sprite = bg4;
                break;
        }
    }

    //���� �ʱ�ȭ
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        CountImage.SetActive(false);

        itemBackgroundImage.sprite = defaultBg;
    }

    #region ���콺 �̺�Ʈ
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if (item.itemType == Item.ItemType.Equipment)
                {
                    //����
                }
                else if (item.itemType == Item.ItemType.Used)
                {
                    //�Ҹ�
                    Debug.Log(item.itemName + " �� ����߽��ϴ�.");
                    SetSlotCount(-1);
                }
                else 
                {
                
                }

            }
        }
    }
    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(UIDragSlot.instance.dragSlot.item, UIDragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null )
        {
            UIDragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            UIDragSlot.instance.dragSlot.ClearSlot();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (UIDragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UIDragSlot.instance.SetColor(0);
        UIDragSlot.instance.dragSlot = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //�������� ������ ��쿡
        if (item != null)
        {
            UIDragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //�������� ������ ��쿡
        if (item != null)   
        {
            UIDragSlot.instance.dragSlot = this;    //�巡�׽� ���� �־���
            UIDragSlot.instance.DragSetImage(itemImage);    //�̹����� ����
            
            UIDragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            slotTooltip.ShowToolTipe(item, transform.position);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        slotTooltip.HideToolTip();
    }

    #endregion
}
