using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CooldownNotifier(float current, float total);

public abstract class Skill : MonoBehaviour
{
    public bool skillEnabled;       // �� ��ų�� Ȱ��ȭ�Ǿ��°�? �� ��ų �����?

    //[SerializeField] protected LayerMask _whatIsEnemy;
    [SerializeField] protected float _cooldown;
    protected float _cooldownTimer;
    protected Player _player;

    [SerializeField] protected PlayerSkill _skillType;

    public event CooldownNotifier OnCooldown;   // ��Ÿ���� ���ư� �� ����Ǵ� �޼���

    protected virtual void Start()
    {
        _player = GameManager.Instance.Player;
    }

    protected virtual void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                _cooldownTimer = 0;
            }

            OnCooldown?.Invoke(current:_cooldownTimer, total:_cooldown);
        }
    }

    public virtual bool AttemptUseSkill()
    {
        if (_cooldownTimer <= 0 && skillEnabled)
        {
            _cooldownTimer = _cooldown;
            UseSkill();
            return true;
        }
        Debug.Log("Skill cooldown or not unlocked!");
        return false;
    }

    public virtual void UseSkill()
    {
        // �� ��ų����. �ǵ�� ����
        SkillManager.Instance.UseSkillFeedback(_skillType);
    }
}
