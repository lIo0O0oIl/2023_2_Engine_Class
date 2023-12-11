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
        // 장착해제
        // 예외처리는 확실하게 해라.
        if (item == null) return;

        if (Keyboard.current.ctrlKey.IsPressed())
        {
            Inventory.Instance.UnEquipItem(item.itemData as ItemDataEquipmentSO);
        }
    }
}
