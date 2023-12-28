using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>, ISaveManager
{
    [SerializeField] private ItemDatabaseSO _itemDB;        // 저장공간

    public List<InventoryItem> stash;       // 잡템창고
    public Dictionary<ItemDataSO, InventoryItem> stashDictionary;       // 중복값 없애려고

    public List<InventoryItem> inven;   // 장비 창고
    public Dictionary<ItemDataSO, InventoryItem> invenDictionary;

    public List<InventoryItem> equipments;
    public Dictionary<ItemDataEquipmentSO, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform _stashSlotParent;
    [SerializeField] private Transform _invenSlotParent;
    [SerializeField] private Transform _equipmentSlotParent;

    private ItemSlotUI[] _stashItemSlots;
    private ItemSlotUI[] _invenItemSlots;
    private EquipmentSlotUI[] _equipmentSlots;

    private void Awake()
    {
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemDataSO, InventoryItem>();
        _stashItemSlots = _stashSlotParent.GetComponentsInChildren<ItemSlotUI>();

        inven = new List<InventoryItem>();
        invenDictionary = new Dictionary<ItemDataSO, InventoryItem>();
        _invenItemSlots = _invenSlotParent.GetComponentsInChildren<ItemSlotUI>();

        equipments = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipmentSO, InventoryItem>();
        _equipmentSlots = _equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();
    }

    private void Start()
    {
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        #region equipment window cleanup
        for (int i = 0; i < _equipmentSlots.Length; ++i)
        {
            EquipmentSlotUI currentSlot = _equipmentSlots[i];
            // 해당 슬롯에 장착할 장비를 내가 List 에 보유하고 있는지를 찾는것.
            ItemDataEquipmentSO slotEquipment = equipmentDictionary.Keys.ToList().Find(x => x.equipmentType == currentSlot.slotType);

            if (slotEquipment != null)
            {
                currentSlot.UpdateSlot(equipmentDictionary[slotEquipment]);
            }
            else
            {
                currentSlot.CleanUpSlot();
            }
        }
        #endregion

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
        // 장비 창고에 해당 item 이 존재하는지 검사해서 count 갯수보다 남아있는 양이 작거나 같다면 아예 아이템을 삭제하고 그렇지 않다면 staskCount 에서 빼고 count 만큰 빼주고
        if (invenDictionary.TryGetValue(item, out InventoryItem inventoryItem))
        {
            if (inventoryItem.stackSize <= count)
            {
                inven.Remove(inventoryItem);
                invenDictionary.Remove(item);
            }
            else
            {
                inventoryItem.RemoveStack(count);
            }
        }

        // 잡템창고에 해당 item 이 존재하는지 검사해서 count 갯수보다 남아있는 양이 작거나 같다면 아예 아이템을 삭제하고 그렇지 않다면 staskCount 에서 빼고 count 만큰 빼주고

        else if (stashDictionary.TryGetValue(item, out InventoryItem stashItem))
        {
            if (stashItem.stackSize <= count)
            {
                stash.Remove(stashItem);
                stashDictionary.Remove(item);
            }
            else
            {
                stashItem.RemoveStack(count);
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

    public void EquipItem(ItemDataSO item)
    {
        ItemDataEquipmentSO newEquipItem = item as ItemDataEquipmentSO;

        if (newEquipItem == null)
        {
            Debug.Log("can not equip!");
            return;
        }

        InventoryItem newItem = new InventoryItem(newEquipItem);

        // 해당 칸에 다른 장비가 장착되어 있는지를 체크해야하고
        ItemDataEquipmentSO oldEquipment = GetEquipmentByType(newEquipItem.equipmentType);
        // 여기다가 그 로직...???

        if (oldEquipment != null)
        {
            UnEquipItem(oldEquipment);
        }
        // 새로 아이템 적용함.

        equipments.Add(newItem);
        equipmentDictionary.Add(newEquipItem, newItem);
        newEquipItem.AddModifiers();        // 해당 장비의 능력치를 플레이어의 스텟에 반영

        RemoveItem(item);
        UpdateSlotUI();
    }

    public ItemDataEquipmentSO GetEquipmentByType(EquipmentType type)
    {
        ItemDataEquipmentSO equipItem = null;

        foreach (var pair in equipmentDictionary)
        {
            if (pair.Key.equipmentType == type)
            {
                equipItem = pair.Key;
                break;
            }
        }
        return equipItem;
    }

    public void UnEquipItem(ItemDataEquipmentSO oldEquipment)
    {
        // 널체크
        if (oldEquipment == null) return;
        // equipmentDictionary 에서 이 녀석이 존재하는지 체크하고
        // 존재하면 equipment 에서 삭제하고
        // 딕셔너리에서도 삭제하고

        if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem invenItem))
        {
            equipments.Remove(invenItem);
            equipmentDictionary.Remove(oldEquipment);

            // RemoveModifiers
            oldEquipment.RemoveModifiers();
            // Additem 으로 인벤토리로 돌려보내자
            AddItem(oldEquipment);
        }
    }

    public void LoadData(GameData data)
    {
        List<ItemDataSO> itemDB = _itemDB.itemList;

        foreach(var pair in data.inventory)
        {
            ItemDataSO item = itemDB.Find(x => x.itemID == pair.Key);
            if (item != null)
            {
                for (int i = 0; i < pair.Value; ++i)
                {
                    AddItem(item);
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.inventory.Clear();
        foreach(var pair in stashDictionary)
        {
            data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach (var pair in invenDictionary)
        {
            data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }
    }
}
