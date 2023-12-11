using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private int _attackCatagoryCount = 3;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");
    private int _facingDirection = 1;

    private CloneSkill _skill;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SetUpClone(CloneSkill skill, Transform originTrm, Vector3 offset)
    {
        _animator.SetInteger(_comboCounterHash, Random.Range(0, _attackCatagoryCount));
        _skill = skill;
        transform.position = originTrm.position + offset;

        // ���� �ٰŸ� ���� �ٶ󺸵��� �Լ��� �ϳ� ������
        FacingClosestTarget();

        // �׸��� ������ �����ǰ�

        //DOTween.Init(transform).SetCapacity(2000, 300);     // �ʹݺ��� ����Ʈ�� ���� ������ְ� �����. ����� ���Ÿ�
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(_skill.cloneDuration);
        seq.Append(_spriteRenderer.DOFade(0, 0.4f));
        seq.AppendCallback(() =>
        {
            Destroy(gameObject);
        });

        //_skill.cloneDuration ��ŭ ������ ���ڿ� 0.4 �� ���� fadeOut �ǰ� �����
    }

    private void FacingClosestTarget()
    {
        Transform target = _skill.FindClosestEnemy(transform, _skill.findEnemyRadius);

        // �̰� ���� �ƴϸ� �� üũ�ؼ� �˸��� �������� ȸ��
        if (target != null)
        {
            if (transform.position.x > target.position.x)
            {
                _facingDirection = -1;      // ���߿� ���ݱ��� �ǰ��ϱ� ���ؼ�
                transform.Rotate(0, 180, 0);
            }
        }
    }

    private void AnimationFinishTrigger()
    {
        
    }
}
