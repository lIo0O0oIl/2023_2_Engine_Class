using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        // ¿Â¬¯«ÿ¡¶
    }
}
