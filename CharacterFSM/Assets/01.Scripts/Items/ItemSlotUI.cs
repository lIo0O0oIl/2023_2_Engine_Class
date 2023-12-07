using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemAmountText;

    [SerializeField] private Sprite _emptySprite;
    public InventoryItem item;

    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;

        if (item != null)
        {
            _itemImage.sprite = item.itemData.itemIcon;

            if (item.stackSize > 1)
            {
                _itemAmountText.text = item.stackSize.ToString();
            }
            else
            {
                _itemAmountText.text = string.Empty;
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;
        _itemImage.sprite = _emptySprite;
        _itemAmountText.text = string.Empty;        // 이 엠티 쓰는 이유는 "" 이거도 할당이 힙에? 들어가서
    }

    // 클릭했을 때
    public void OnPointerDown(PointerEventData eventData)
    {
        // 예외처리는 확실하게 해라.
        if (item == null) return;

        if (!Keyboard.current.ctrlKey.IsPressed())
        {
            return;     // 컨트롤 키를 누른 상태였어야 함
        }

        if (item.itemData.itemType == ItemType.Equipment)
        {
            // 장비해제
        }

        Inventory.Instance.RemoveItem(item.itemData);
    }
}
