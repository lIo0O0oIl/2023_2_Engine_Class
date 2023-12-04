using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    damage,
    criticalChance,
    criticalDamage,
    fireDamage,
    ignitePercent,
    iceDamage,
    chillPercent,
    thunderDamage,
    shockPercent
}

// �긦 ����� ���� �����ϱ�.
public abstract class EntityStat : ScriptableObject
{
    #region ���� �����
    [Header("Major stat")]
    public Stat strength; // 1����Ʈ�� ������ ����, ũ���� 1%
    public Stat agility; // 1����Ʈ�� ȸ�� 1%, ũ��Ƽ�� 1%
    public Stat intelligence; // 1����Ʈ�� ���������� 1����, �������� 3����, ��Ʈ �������� ������ 10% ����(����10�� ��Ʈ�� 10�� ����)
    public Stat vitality; // 1����Ʈ�� ü�� 5����.


    [Header("Defensive stats")]
    public Stat maxHealth; //ü��
    public Stat armor; //��
    public Stat evasion; //ȸ�ǵ�
    public Stat magicResistance; //�������

    [Header("Offensive stats")]
    public Stat damage;
    public Stat criticalChance;
    public Stat criticalDamage;


    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat ignitePercent;
    public Stat iceDamage;
    public Stat chillPercent;
    public Stat thunderDamage;
    public Stat shockPercent;
    #endregion

    protected Player _owner;
    protected Dictionary<StatType, Stat> _statDictionary = new Dictionary<StatType, Stat>();

    public virtual void SetOwner(Player owner)
    {
        _owner = owner;
    }
    
    // ����
    public virtual void IncreaseStatBy(int modifyValue, float duration, Stat statToModify)
    {
        _owner.StartCoroutine(StatModityCoroution(modifyValue, duration, statToModify));
    }

    protected IEnumerator StatModityCoroution(int modifyValue, float duration, Stat statToModify)
    {
        statToModify.AddModifier(modifyValue);
        yield return new WaitForSeconds(duration);
        statToModify.RemoveModifier(modifyValue);
    }
}
