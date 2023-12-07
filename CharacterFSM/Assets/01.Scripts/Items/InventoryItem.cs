using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    // 이 아이템이 몇 개 들어가있어?
    public ItemDataSO itemData;
    public int stackSize;

    public InventoryItem(ItemDataSO itemData)
    {
        this.itemData = itemData;
        AddStack();
    }

    public void AddStack()
    {
        ++stackSize;
    }

    public void RemoveStack(int count = 1)
    {
        stackSize -= count;
    }
}
