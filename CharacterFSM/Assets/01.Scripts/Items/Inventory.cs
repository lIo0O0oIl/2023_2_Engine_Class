using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>, ISaveManager
{
    [SerializeField] private ItemDatabaseSO _itemDB;        // �������

    public List<InventoryItem> stash;       // ����â��
    public Dictionary<ItemDataSO, InventoryItem> stashDictionary;       // �ߺ��� ���ַ���

    public List<InventoryItem> inven;   // ��� â��
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
            // �ش� ���Կ� ������ ��� ���� List �� �����ϰ� �ִ����� ã�°�.
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
        // ��� â�� �ش� item �� �����ϴ��� �˻��ؼ� count �������� �����ִ� ���� �۰ų� ���ٸ� �ƿ� �������� �����ϰ� �׷��� �ʴٸ� staskCount ���� ���� count ��ū ���ְ�
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

        // ����â�� �ش� item �� �����ϴ��� �˻��ؼ� count �������� �����ִ� ���� �۰ų� ���ٸ� �ƿ� �������� �����ϰ� �׷��� �ʴٸ� staskCount ���� ���� count ��ū ���ְ�

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

    public void EquipItem(ItemDataSO item)
    {
        ItemDataEquipmentSO newEquipItem = item as ItemDataEquipmentSO;

        if (newEquipItem == null)
        {
            Debug.Log("can not equip!");
            return;
        }

        InventoryItem newItem = new InventoryItem(newEquipItem);

        // �ش� ĭ�� �ٸ� ��� �����Ǿ� �ִ����� üũ�ؾ��ϰ�
        ItemDataEquipmentSO oldEquipment = GetEquipmentByType(newEquipItem.equipmentType);
        // ����ٰ� �� ����...???

        if (oldEquipment != null)
        {
            UnEquipItem(oldEquipment);
        }
        // ���� ������ ������.

        equipments.Add(newItem);
        equipmentDictionary.Add(newEquipItem, newItem);
        newEquipItem.AddModifiers();        // �ش� ����� �ɷ�ġ�� �÷��̾��� ���ݿ� �ݿ�

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
        // ��üũ
        if (oldEquipment == null) return;
        // equipmentDictionary ���� �� �༮�� �����ϴ��� üũ�ϰ�
        // �����ϸ� equipment ���� �����ϰ�
        // ��ųʸ������� �����ϰ�

        if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem invenItem))
        {
            equipments.Remove(invenItem);
            equipmentDictionary.Remove(oldEquipment);

            // RemoveModifiers
            oldEquipment.RemoveModifiers();
            // Additem ���� �κ��丮�� ����������
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
