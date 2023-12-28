using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int gold;
    public int exp;

    public SerializableDictionary<string, int> skillTree;
    public SerializableDictionary<string, int> inventory;

    public GameData()
    {
        gold = 0;
        exp = 0;
        skillTree = new SerializableDictionary<string, int>();
        inventory = new SerializableDictionary<string, int>();
    }

    // ��ųƮ��, �κ��丮 ����(�ȸ����)
}
