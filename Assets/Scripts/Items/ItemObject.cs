using Unity.Properties;
using UnityEngine;

public enum ItemType
{
    Food,
    Equipment,
    Default
}

//public enum Attrirbutes
//{
//    Agility,
//    Intellect,
//    Stamina,
//    Strngth
//}

public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public Sprite uiDisplay;
    public ItemType type;

    [TextArea(15, 3)]
    public string description;
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Id;
    }
}

//public class ItemBuff
//{
//    public AttributesScope attribute;
//    public int value;
//    public int min;
//    public int max;
//    public ItemBuff(int _min, int _max)
//    {
//        min = _min; 
//        max = _max;
//    }
//}