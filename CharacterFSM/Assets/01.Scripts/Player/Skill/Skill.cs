using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CooldownNotifier(float current, float total);

public abstract class Skill : MonoBehaviour
{
    public bool skillEnabled;       // 이 스킬이 활성화되었는가? 이 스킬 얻었니?

    [SerializeField] protected LayerMask _whatIsEnemy;
    [SerializeField] protected float _cooldown;
    protected float _cooldownTimer;
    protected Player _player;

    [SerializeField] protected PlayerSkill _skillType;

    [SerializeField] protected int _collisionDetectCount;      // 몇 개까지 충돌검사할거냐 
    protected Collider2D[] _collisionColliders;

    public event CooldownNotifier OnCooldown;   // 쿨타임이 돌아갈 때 발행되는 메세지

    protected virtual void Start()
    {
        _player = GameManager.Instance.Player;
        _collisionColliders = new Collider2D[_collisionDetectCount];            // 갯수만큼 미리 만들어서
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
        // 나 스킬썼음. 피드백 해줘
        SkillManager.Instance.UseSkillFeedback(_skillType);
    }

    public virtual Transform FindClosestEnemy(Transform checkTrm, float radius)
    {
        Transform target = null;

        int cnt = Physics2D.OverlapCircle(checkTrm.position, radius, new ContactFilter2D { layerMask = _whatIsEnemy, useLayerMask = true }, _collisionColliders);       // 오버랩써클 쓰는 방법

        //Collider2D[] colliders = Physics2D.OverlapCircleAll(checkTrm.position, radius, _whatIsEnemy);

        float closestDistance = Mathf.Infinity;

        //foreach (Collider2D collider in colliders)
        for (int i = 0; i < cnt; ++i)
        {
            Collider2D collider = _collisionColliders[i];
            float distance = Vector2.Distance(checkTrm.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = collider.transform;
            }
        }
        return target;
    }
}
