using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Stat/Player")]
public class PlayerStat : EntityStat
{
    protected void OnEnable()       // 온에이블에서야 SO 가 돌아감.??
    {
        Type playerStatType = typeof(PlayerStat);

        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            string statName = statType.ToString();
            FieldInfo statField = playerStatType.GetField(statName);

            if (statField == null)
            {
                Debug.LogError($"There is no stat field! error : {statName}");
            }
            else
            {
                Stat targetStat = statField.GetValue(this) as Stat;
                _statDictionary.Add(statType, targetStat);
            }
        }
    }

    public Stat GetStatByType(StatType type)
    {
        return _statDictionary[type];
    }
}
