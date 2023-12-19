using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>, ISaveManager
{
    [SerializeField] private int _currentGold;
    [SerializeField] private int _currentExp;

    public void LoadData(GameData data)
    {
        _currentExp = data.exp;
        _currentGold = data.gold;
    }

    public void SaveData(ref GameData data)
    {
        data.exp = _currentExp;
        data.gold = _currentGold;
    }
}
