using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EquipmentSlotUI : ItemSlotUI
{
    public EquipmentType slotType;

#if UNITY_EDITOR
    private void OnValidate()
    {
        gameObject.name = $"EquipSlot [{slotType.ToString()}]";
    }
#endif

    public override void OnPointerDown(PointerEventData eventData)
    {
        //base.OnPointerDown(eventData);
        // ��������
        // ����ó���� Ȯ���ϰ� �ض�.
        if (item == null) return;

        if (Keyboard.current.ctrlKey.IsPressed())
        {
            Inventory.Instance.UnEquipItem(item.itemData as ItemDataEquipmentSO);
        }
    }
}
