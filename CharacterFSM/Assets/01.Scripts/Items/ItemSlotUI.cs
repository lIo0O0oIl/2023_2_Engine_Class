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
        _itemAmountText.text = string.Empty;        // �� ��Ƽ ���� ������ "" �̰ŵ� �Ҵ��� ����? ����
    }

    // Ŭ������ ��
    public void OnPointerDown(PointerEventData eventData)
    {
        // ����ó���� Ȯ���ϰ� �ض�.
        if (item == null) return;

        if (!Keyboard.current.ctrlKey.IsPressed())
        {
            return;     // ��Ʈ�� Ű�� ���� ���¿���� ��
        }

        if (item.itemData.itemType == ItemType.Equipment)
        {
            // �������
        }

        Inventory.Instance.RemoveItem(item.itemData);
    }
}
