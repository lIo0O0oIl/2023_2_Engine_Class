using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private CrystalSkill _skill;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private readonly int _hashExplodeTrigger = Animator.StringToHash("explode");

    private float _lifeTimer;
    private bool _isDestroyed = false;

    private Transform _targetTrm = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetUpCrystal(CrystalSkill crystalSkill)
    {
        _skill = crystalSkill;
        _lifeTimer = crystalSkill.timeOut;
        _isDestroyed = false;
    }

    private void Update()
    {
        if (_skill.canMove)
        {
            // 만약 현재 타겟이 존재하지 않으면 가장 가까운 타겟을 찾아서 넣어주고 타겟이 존재한다면 타겟을 쫒아간다.
            // 타겟이 일정거리(1f) 내로 들어온다면 폭발한다.
            if (_targetTrm == null)
            {
                _targetTrm = _skill.FindClosestEnemy(transform, 10);
            }
                ChaseToTarget();
        }

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0 && !_isDestroyed)
        {
            EndOfCrystal();
        }
    }

    private void ChaseToTarget()
    {
        if (_isDestroyed) return;
        transform.position = Vector2.MoveTowards(transform.position, _targetTrm.position, _skill.moveSpeed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, _targetTrm.position);
        if (distance < 1f)
        {
            EndOfCrystal();
        }
    }

    public void EndOfCrystal()
    {
        _isDestroyed = true;
        _skill.UnlinkCrystal();

        if (_skill.canExplode)
        {
            transform.DOScale(Vector3.one * 2.5f, 0.1f);
            _animator.SetTrigger(_hashExplodeTrigger);
        }
        else
        {
            DestroySelf();
        }

    }

    private void DestroySelf(float tweenTime = 0.4f)        // 만들어지고 시간이 지나면 사라지게 
    {
        _spriteRenderer.DOFade(0f, tweenTime).OnComplete(() => Destroy(gameObject));
    }

    private void EndOfExplosionAnimation()
    {
        // 여기에 적을 찾아서 데미지를 입혀주는 부분이 있어야 해.
        transform.DOKill();
        DestroySelf(0.1f);
    }
}