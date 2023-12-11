using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("clone info")]
    // ������
    [SerializeField] private CloneSkillController _clonePrefab;
    [SerializeField] private bool _createCloneOnDashStart;
    [SerializeField] private bool _createCloneOnDashEnd;
    [SerializeField] private bool _createCloneOnCounterAttack;      // ���� ��.

    public float cloneDuration;
    public float findEnemyRadius = 5f;      // ���� ã�Ƽ� �� �������� ȸ��


    public void CreateClone(Transform originTrm, Vector3 offset)
    {
        // Ŭ�� ������ֱ�
        CloneSkillController newClone = Instantiate(_clonePrefab);
        newClone.SetUpClone(this, originTrm, offset);
    }

    public void CreateCloneOnDashStart()
    {
        if (_createCloneOnDashStart)
        {
            CreateClone(_player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashEnd()
    {
        if (_createCloneOnDashEnd)
        {
            CreateClone(_player.transform, Vector3.zero);
        }
    }
}
