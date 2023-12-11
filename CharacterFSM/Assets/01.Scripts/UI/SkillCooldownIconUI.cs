using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownIconUI : MonoBehaviour
{
    [SerializeField] private PlayerSkill _skillType;
    [SerializeField] private Image _cooldownImage;
    //[SerializeField] private Text _cooldownText;      // 텍스트로도 표시하고 싶다면~

    private Skill _targetSkill;

    private void Start()
    {
        _targetSkill = SkillManager.Instance.GetSkill(_skillType);
        _cooldownImage.fillAmount = 0;
        _targetSkill.OnCooldown += HandleCooldown;
    }

    private void HandleCooldown(float current, float total)     // 사용가능하다면 델리게이트 쓰는게 좋음. 가독성 부분에서
    {
        _cooldownImage.fillAmount = current / total;
        //_cooldownText.text = current.ToString();        // 소수 2번째까지 하고 싶은데엥잉
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
