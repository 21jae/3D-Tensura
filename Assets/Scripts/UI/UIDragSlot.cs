using UnityEngine;
using UnityEngine.UI;

public class UIDragSlot : MonoBehaviour
{
    //아이템 이미지
    [SerializeField] private Image imageItem;
    static public UIDragSlot instance;
    public UISlot dragSlot;

    private void Awake()
    {
        instance = this;
    }


    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
