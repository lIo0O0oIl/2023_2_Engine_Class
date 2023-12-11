using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownIconUI : MonoBehaviour
{
    [SerializeField] private PlayerSkill _skillType;
    [SerializeField] private Image _cooldownImage;
    //[SerializeField] private Text _cooldownText;      // �ؽ�Ʈ�ε� ǥ���ϰ� �ʹٸ�~

    private Skill _targetSkill;

    private void Start()
    {
        _targetSkill = SkillManager.Instance.GetSkill(_skillType);
        _cooldownImage.fillAmount = 0;
        _targetSkill.OnCooldown += HandleCooldown;
    }

    private void HandleCooldown(float current, float total)     // ��밡���ϴٸ� ��������Ʈ ���°� ����. ������ �κп���
    {
        _cooldownImage.fillAmount = current / total;
        //_cooldownText.text = current.ToString();        // �Ҽ� 2��°���� �ϰ� ����������
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_skillType != 0)
        {
            gameObject.name = $"SkillCooldownUI[{_skillType.ToString()}]";
        }
    }
#endif
}
