using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite itemImage;
    public GameObject itemPrefab;

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

    void Start()
    {
        itemType = ItemType.Equipment;
    }

    void Update()
    {
        
    }
}
