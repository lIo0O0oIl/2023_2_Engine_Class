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

            if (_itemAmountText == null) return;

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

        if (_itemAmountText == null) return;
        _itemAmountText.text = string.Empty;        // �� ��Ƽ ���� ������ "" �̰ŵ� �Ҵ��� ����? ����
    }

    // Ŭ������ ��
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // ����ó���� Ȯ���ϰ� �ض�.
        if (item == null) return;

        if (item.itemData.itemType == ItemType.Equipment)
        {
            // ������� �Ǵ� ����
            Inventory.Instance.EquipItem(item.itemData);
            return;
        }

        if (!Keyboard.current.ctrlKey.IsPressed())
        {
            return;     // ��Ʈ�� Ű�� ���� ���¿���� ��
        }

        Inventory.Instance.RemoveItem(item.itemData);
    }
}
