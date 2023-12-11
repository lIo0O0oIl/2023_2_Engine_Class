using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("clone info")]
    // 프리텝
    [SerializeField] private CloneSkillController _clonePrefab;
    [SerializeField] private bool _createCloneOnDashStart;
    [SerializeField] private bool _createCloneOnDashEnd;
    [SerializeField] private bool _createCloneOnCounterAttack;      // 만들어만 둠.

    public float cloneDuration;
    public float findEnemyRadius = 5f;      // 적을 찾아서 적 방향으로 회전


    public void CreateClone(Transform originTrm, Vector3 offset)
    {
        // 클론 만들어주기
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
