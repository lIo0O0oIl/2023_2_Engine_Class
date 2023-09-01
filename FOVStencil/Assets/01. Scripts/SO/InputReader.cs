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

    private void OnEnable()     // 어디서 싱글턴 만들어서 그걸 집어넣고
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);        // 예가 입력 받고 있는 것들을 이 스크립트에서 받아와서 사용함

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
        Movement?.Invoke(value);        // 이건 노멀라이즈 된 값
    }
}
