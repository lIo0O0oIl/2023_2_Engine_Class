using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill, ISaveManager
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

    void ISaveManager.LoadData(GameData data)
    {
        if (data.skillTree.TryGetValue("enable", out int value))
        {
            skillEnabled = value == 1;
        }
        if (data.skillTree.TryGetValue("clone_start", out int startvalue))
        {
            _createCloneOnDashStart = startvalue == 1;
        }
        if (data.skillTree.TryGetValue("clone_end", out int endvalue))
        {
            _createCloneOnDashEnd = endvalue == 1;
        }
        if (data.skillTree.TryGetValue("clone_counter", out int countervalue))
        {
            _createCloneOnCounterAttack = countervalue == 1;
        }
    }

    void ISaveManager.SaveData(ref GameData data)
    {
        data.skillTree.Clear();
        data.skillTree.Add("enable", skillEnabled ? 1 : 0);
        data.skillTree.Add("clone_start", _createCloneOnDashStart ? 1 : 0); 
        data.skillTree.Add("clone_end", _createCloneOnDashEnd ? 1 : 0); 
        data.skillTree.Add("clone_counter", _createCloneOnCounterAttack ? 1 : 0); 
    }
}
