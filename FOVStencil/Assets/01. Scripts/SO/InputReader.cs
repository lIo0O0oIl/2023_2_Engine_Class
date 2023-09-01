using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(menuName ="SO/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> Movement;
    public Vector3 AimPosition { get; private set; }

    private Controls _controls;

    private void OnEnable()     // ��� �̱��� ���� �װ� ����ְ�
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);        // ���� �Է� �ް� �ִ� �͵��� �� ��ũ��Ʈ���� �޾ƿͼ� �����

            _controls.Player.Enable();
        }
    }

    public void OnAIM(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }

    public void OnMovement(InputAction.CallbackContext context) 
    {
        Vector2 value = context.ReadValue<Vector2>();
        Movement?.Invoke(value);        // �̰� ��ֶ����� �� ��
    }
}
