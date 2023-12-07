using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{
    public List<InventoryItem> stash;       // ����â��
    public Dictionary<ItemDataSO, InventoryItem> stashDictionary;       // �ߺ��� ���ַ���

    public List<InventoryItem> inven;   // ��� â��
    public Dictionary<ItemDataSO, InventoryItem> invenDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform _stashSlotParent;
    [SerializeField] private Transform _invenSlotParent;

    private ItemSlotUI[] _stashItemSlots;
    private ItemSlotUI[] _invenItemSlots;

    private void Awake()
    {
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemDataSO, InventoryItem>();
        _stashItemSlots = _stashSlotParent.GetComponentsInChildren<ItemSlotUI>();

        inven = new List<InventoryItem>();
        invenDictionary = new Dictionary<ItemDataSO, InventoryItem>();
        _invenItemSlots = _invenSlotParent.GetComponentsInChildren<ItemSlotUI>();
    }

    private void Start()
    {
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        #region clean up slots
        for (int i = 0; i < _stashItemSlots.Length; ++i)
        {
            _stashItemSlots[i].CleanUpSlot();
        }
        for (int i = 0; i < _invenItemSlots.Length; ++i)
        {
            _invenItemSlots[i].CleanUpSlot();
        }
        #endregion

        #region redraw slots
        for (int i = 0; i < stash.Count; ++i)
        {
            _stashItemSlots[i].UpdateSlot(stash[i]);
        }
        for (int i = 0; i < inven.Count; ++i)
        {
            _invenItemSlots[i].UpdateSlot(inven[i]);
        }
        #endregion
    }

    public void AddItem(ItemDataSO item)
    {
        if (item.itemType == ItemType.Equipment)
        {
            // ����� ��� ���� ������� ������
            AddToInventory(item);
        }
        else if (item.itemType == ItemType.Material)
        {
            AddToStash(item);
        }

        UpdateSlotUI();
    }


    public void RemoveItem(ItemDataSO item, int count = 1)
    {
        // ����â�� �ش� item �� �����ϴ��� �˻��ؼ� count �������� �����ִ� ���� �۰ų� ���ٸ� �ƿ� �������� �����ϰ� �׷��� �ʴٸ� staskCount ���� ���� count ��ū ���ְ�

        if (stashDictionary.TryGetValue(item, out InventoryItem inventoryItem))
        {
            if (inventoryItem.stackSize <= count)
            {
                stash.Remove(inventoryItem);
                stashDictionary.Remove(item);
            }
            else
            {
                inventoryItem.RemoveStack(count);
            }
        }

        UpdateSlotUI();
    }

    private void AddToInventory(ItemDataSO item)
    {
        // �̹� �ش� �������� �κ��丮�� �ִٸ� ��ġ�� �ø��� �׷��� �ʳ��� ���� ����ϰ�, ���߿� ��� �ߺ����� �����ų� ���ָ� ����. ���� ��� 2���̻� ������? 
        if (invenDictionary.TryGetValue(item, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack();       // ���� �ϳ� ����
        }
        else
        {
            InventoryItem newInventoryItem = new InventoryItem(item);
            inven.Add(newInventoryItem);
            invenDictionary.Add(item, newInventoryItem);
        }
    }

    private void AddToStash(ItemDataSO item)
    {
        // �̹� �ش� �������� �κ��丮�� �ִٸ� ��ġ�� �ø��� �׷��� �ʳ��� ���� ����ϰ�
        if (stashDictionary.TryGetValue(item, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack();       // ���� �ϳ� ����
        }
        else
        {
            InventoryItem newInventoryItem = new InventoryItem(item);
            stash.Add(newInventoryItem);
            stashDictionary.Add(item, newInventoryItem);
        }
    }
}
