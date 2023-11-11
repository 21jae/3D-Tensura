using System;
using UnityEngine;

public class UICharacterEquipment : MonoBehaviour
{
    [SerializeField] private UISlot_Equipment hatSlot;
    [SerializeField] private UISlot_Equipment armorSlot;
    [SerializeField] private UISlot_Equipment accessorySlot;
    [SerializeField] private UISlot_Equipment weaponSlot;

    public PlayerController playerController;

    public void EquipmentItem(Item item)
    {
        switch (item.eqipmentType)
        {
            case Item.EquipmentType.Hat:
                EquipToSlot(hatSlot, item);
                break;
            case Item.EquipmentType.Armor:
                EquipToSlot(armorSlot, item);
                break;
            case Item.EquipmentType.Accessory:
                EquipToSlot(accessorySlot, item);
                break;
            case Item.EquipmentType.Weapon:
                EquipToSlot(weaponSlot, item);
                break;
        }
    }

    private void EquipToSlot(UISlot_Equipment slot, Item item)
    {
        if (slot.item != null)
        {
            //���� ���Կ� �̹� �ٸ� �������� �ִٸ� ��ü
            //UIInventory.instance.ReturnItemToInventory(slot.item);
        }
        slot.SetItem(item);

        UpdateCharacterState(item);
        //���� �������
    }

    private void UpdateCharacterState(Item addStats)
    {
        float increaseAttackPower = addStats.equipmentStats.attackPower;
        CharacterStatManager.instance.ModifyAttackPower(increaseAttackPower);

        float modifiedMaxHP = addStats.equipmentStats.health;
        CharacterStatManager.instance.ModifyMaxHealth(modifiedMaxHP);


        float modifiedDefense = addStats.equipmentStats.defense;
        CharacterStatManager.instance.ModifyDefence(modifiedDefense);


        float increasePercentage = addStats.equipmentStats.percentageUp * CharacterStatManager.instance.currentData.currentAttackPower;
        CharacterStatManager.instance.ModifyAttackPower(increasePercentage);
        playerController.playerStatManager.ModifyAttackPower(increasePercentage);


        DebugPlayerStats();
    }

    private void DebugPlayerStats()
    {
        Debug.Log($" MaxHP : {CharacterStatManager.instance.currentData.currentMaxHP}, " +
                     $" HP : {CharacterStatManager.instance.currentData.currentHP}, " +
                    $" ATK : {CharacterStatManager.instance.currentData.currentAttackPower}, " +
                    $" DEF : {CharacterStatManager.instance.currentData.currentDefense} ");
    }
}
