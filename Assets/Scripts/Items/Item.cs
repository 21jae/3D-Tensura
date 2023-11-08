using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string itemDesc;
    public ItemType itemType;
    public EquipmentType eqipmentType;
    public Sprite itemImage;
    public GameObject itemPrefab;

    [field: SerializeField] public EquipmentStats equipmentStats { get; private set; }

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }
    
    public enum EquipmentType
    {
        None,
        Hat,
        Armor,
        Accessory,
        Weapon
    }


    void Start()
    {
        itemType = ItemType.Equipment;
    }

    public bool IsEquipmentType(EquipmentType type)
    {
        return itemType == ItemType.Equipment && eqipmentType == type;
    }

}
