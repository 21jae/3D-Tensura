using UnityEngine;
using UnityEngine.UI;

public class UIDragSlot : MonoBehaviour
{
    static public UIDragSlot instance;
    public UISlot dragSlot;

    private void Awake()
    {
        instance = this;
    }

    //������ �̹���
    [SerializeField] private Image imageItem;

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
