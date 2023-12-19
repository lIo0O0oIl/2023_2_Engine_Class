using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private CrystalSkillController _crystalPrefab;
    public float timeOut = 5f;

    private CrystalSkillController _currentCrystal;

    public int damage = 5;

    public bool canExplode;
    public float explosionRadius = 3f;

    public bool canMove;
    public float moveSpeed;
    public float findEnemyRadius = 10f;

    public override bool AttemptUseSkill()
    {
        if (_cooldownTimer <= 0 && skillEnabled && _currentCrystal == null)
        {
            UseSkill();
            return true;
        }

        if (_currentCrystal != null)
        {
            // �̹� ��ȯ�� ũ����Ż�� �ִٸ� ���� ���� ����� �����ش�.
            WarpToCrystalPosition();
        }

        Debug.Log("Skill cooldown or locked");
        return false;
    }

    private void WarpToCrystalPosition()
    {
        // �÷��̾�� ũ����Ż ��ġ�� ��ȯ�ϰ� ũ����Ż�� �������ش�.
        Vector2 playerPos = _player.transform.position;
        _player.transform.position = _currentCrystal.transform.position;
        _currentCrystal.transform.position = playerPos;



        _currentCrystal.EndOfCrystal();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (_currentCrystal == null)
        {
            CreateCrystal(_player.transform.position);
        }
    }

    public void UnlinkCrystal()
    {
        _cooldownTimer -= _cooldown;
        _currentCrystal = null;
    }

    private void CreateCrystal(Vector3 position)
    {
        _currentCrystal = Instantiate(_crystalPrefab, position, Quaternion.identity);
        _currentCrystal.SetUpCrystal(this);
    }
}
