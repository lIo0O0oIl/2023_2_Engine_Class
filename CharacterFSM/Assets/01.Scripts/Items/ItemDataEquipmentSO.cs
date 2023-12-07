using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(menuName ="SO/Item/Equipment")]
public class ItemDataEquipmentSO : ItemDataSO
{
    public EquipmentType equipmentType;

    [TextArea] public string itemEffectDescription;

    #region ���� �����
    [Header("Major stat")]
    public int strength; // 1����Ʈ�� ������ ����, ũ���� 1%
    public int agility; // 1����Ʈ�� ȸ�� 1%, ũ��Ƽ�� 1%
    public int intelligence; // 1����Ʈ�� ���������� 1����, �������� 3����, ��Ʈ �������� ������ 10% ����(����10�� ��Ʈ�� 10�� ����)
    public int vitality; // 1����Ʈ�� ü�� 5����.


    [Header("Defensive stats")]
    public int maxHealth; //ü��
    public int armor; //��
    public int evasion; //ȸ�ǵ�
    public int magicResistance; //�������

    [Header("Offensive stats")]
    public int damage;
    public int criticalChance;
    public int criticalDamage;


    [Header("Magic stats")]
    public int fireDamage;
    public int ignitePercent;
    public int iceDamage;
    public int chillPercent;
    public int thunderDamage;
    public int shockPercent;
    #endregion

    protected Dictionary<StatType, FieldInfo> _fieldInfoDictionary;

    private void OnEnable()
    {
        _fieldInfoDictionary = new Dictionary<StatType, FieldInfo>();

        Type itemType = typeof(ItemDataEquipmentSO);
        
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            FieldInfo itemStatField = itemType.GetField(statType.ToString());

            if (itemStatField == null)
            {
                Debug.LogError($"There is no stat field i {statType.ToString()}-{name}");
            }
            else
            {
                _fieldInfoDictionary.Add(statType, itemStatField);
            }
        }
    }

    public void AddModifiers()
    {
        PlayerStat playerStat = GameManager.Instance.Player.Stat;

        if (playerStat == null) return;

        foreach (var pair in _fieldInfoDictionary)
        {
            Stat stat = playerStat.GetStatByType(pair.Key);
            int modifyValue = (int)pair.Value.GetValue(this);
            stat.AddModifier(modifyValue);
        }
    }

    public void RemoveModifiers()
    {
        // ������̾� ����
    }
}
