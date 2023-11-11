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
            //만약 슬롯에 이미 다른 아이템이 있다면 교체
            //UIInventory.instance.ReturnItemToInventory(slot.item);
        }
        slot.SetItem(item);

        UpdateCharacterState(item);
        //장착 사운드재생
    }

    private void UpdateCharacterState(Item addStats)
    {
        float increaseAttackPower = addStats.equipmentStats.attackPower;
        //playerController.playerStatManager.ModifyAttackPower(increaseAttackPower);
        CharacterStatManager.instance.ModifyAttackPower(increaseAttackPower);

        float modifiedMaxHP = addStats.equipmentStats.health;
        //playerController.playerStatManager.ModifyHealth(modifiedMaxHP);
        CharacterStatManager.instance.ModifyHealth(modifiedMaxHP);


        float modifiedDefense = addStats.equipmentStats.defense;
        //playerController.playerStatManager.ModifyDefence(modifiedDefense);
        CharacterStatManager.instance.ModifyDefence(modifiedDefense);


        float increasePercentage = addStats.equipmentStats.percentageUp * playerController.playerStatManager.currentData.currentAttackPower;
        //playerController.playerStatManager.ModifyAttackPower(increasePercentage);
        CharacterStatManager.instance.ModifyAttackPower(increasePercentage);


        DebugPlayerStats();
    }

    private void DebugPlayerStats()
    {
        Debug.Log($" MaxHP : {playerController.playerStatManager.currentData.currentMaxHP}, " +
                     $" HP : {playerController.playerStatManager.currentData.currentHP}, " +
                    $" ATK : {playerController.playerStatManager.currentData.currentAttackPower}, " +
                    $" DEF : {playerController.playerStatManager.currentData.currentDefense} ");
    }
}
