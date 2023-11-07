using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler ,IDropHandler 
{
    [Header("Background Images")]
    [SerializeField] private Image itemBackgroundImage; //아이템 배경이미지
    [SerializeField] private Sprite defaultBg;  //기본배경
    [SerializeField] private Sprite bg1;    //equipment 배경
    [SerializeField] private Sprite bg2;    //Used 배경
    [SerializeField] private Sprite bg3;    //Ingredient 배경
    [SerializeField] private Sprite bg4;    //ETC배경
    [SerializeField] private UISlotToolTip slotTooltip;

    public Item item;
    public int itemCount;
    public Image itemImage;

    [SerializeField] private GameObject CountImage;
    [SerializeField] private Text text_Count;


    //이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //아이템 획득
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

        SetColor(1);    //아이템 보여주기
    }


    //아이템 갯수 조정
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

    //슬롯 초기화
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

    #region 마우스 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if (item.itemType == Item.ItemType.Equipment)
                {
                    //장착
                }
                else if (item.itemType == Item.ItemType.Used)
                {
                    //소모
                    Debug.Log(item.itemName + " 를 사용했습니다.");
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
        //아이템을 눌렀을 경우에
        if (item != null)
        {
            UIDragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //아이템을 눌렀을 경우에
        if (item != null)   
        {
            UIDragSlot.instance.dragSlot = this;    //드래그시 전부 넣어줌
            UIDragSlot.instance.DragSetImage(itemImage);    //이미지도 변경
            
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
