using UnityEngine;
using UnityEngine.UI;

public class UISlotToolTip : MonoBehaviour
{
    [SerializeField] private GameObject tooltipBase;

    [SerializeField] private Text text_ItemName;
    [SerializeField] private Text text_ItemDesc;
    [SerializeField] private Text text_ItemUsed;

    public void ShowToolTipe(Item _item, Vector3 _pos)
    {
        tooltipBase.SetActive(true);
        _pos += new Vector3(tooltipBase.GetComponent<RectTransform>().rect.width * 0.5f, -tooltipBase.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        tooltipBase.transform.position = _pos;

        text_ItemName.text = _item.itemName;
        text_ItemDesc.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Equipment)
        {
            text_ItemUsed.text = "우클릭 - 장착";
        }
        else if (_item.itemType == Item.ItemType.Used)
        {
            text_ItemUsed.text = "우클릭 - 소비";
        }
        else
        {
            text_ItemUsed.text = "";
        }
    }

    public void HideToolTip()
    {
        tooltipBase.SetActive(false);
    }
}
