using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletUI : MonoBehaviour
{
    private int _currentCnt;
    private int _maxCnt;
    private List<VisualElement> _bulletList;

    public int BulletCount
    {
        get => _currentCnt;
        set
        {
            _currentCnt = Mathf.Clamp(value, 0, _maxCnt);
            DrawBullet();
        }
    }

    public BulletUI(VisualElement root, int maxCount)
    {
        _maxCnt = maxCount;
        _bulletList = root.Query<VisualElement>(className: "bullet").ToList();      // �� Ŭ������ ������ �ִ� �ֵ��� ����Ʈ�� ��������
    }

    private void DrawBullet()
    {
        for (int i = 0; i < _bulletList.Count; i++)
        {
            if (i < _currentCnt - 1)
            {
                _bulletList[i].AddToClassList("on");
            }
            else
            {
                _bulletList[i].RemoveFromClassList("on");
            }
        }
    }
}
