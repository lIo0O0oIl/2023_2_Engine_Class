using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{
    public List<InventoryItem> stash;       // 잡템창고
    public Dictionary<ItemDataSO, InventoryItem> stashDictionary;       // 중복값 없애려고

    public List<InventoryItem> inven;   // 장비 창고
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
            // 장비일 경우 따로 장비간으로 보내고
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
        // 잡템창고에 해당 item 이 존재하는지 검사해서 count 갯수보다 남아있는 양이 작거나 같다면 아예 아이템을 삭제하고 그렇지 않다면 staskCount 에서 빼고 count 만큰 빼주고

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
        // 이미 해당 아이템이 인벤토리상에 있다면 수치를 올리고 그렇지 않나면 새로 등록하고, 나중에 장비 중복으로 받을거냐 해주면 좋음. 같은 장비 2개이상 먹을래? 
        if (invenDictionary.TryGetValue(item, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack();       // 스택 하나 증가
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
        // 이미 해당 아이템이 인벤토리상에 있다면 수치를 올리고 그렇지 않나면 새로 등록하고
        if (stashDictionary.TryGetValue(item, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack();       // 스택 하나 증가
        }
        else
        {
            InventoryItem newInventoryItem = new InventoryItem(item);
            stash.Add(newInventoryItem);
            stashDictionary.Add(item, newInventoryItem);
        }
    }
}
