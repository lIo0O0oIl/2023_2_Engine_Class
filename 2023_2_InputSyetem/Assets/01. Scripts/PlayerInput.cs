using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private UserInputAction _inputAction;

    public Action<Vector2> OnMovement;
    public Action OnJump;

    private void Awake()
    {
        _inputAction = new UserInputAction();
        _inputAction.Player.Enable();
        _inputAction.Player.Jump.performed += Jump;

        _inputAction.UI.Submit.performed += UIPerformPress;

        _inputAction.Player.Disable();

        var op = _inputAction.Player.Jump.PerformInteractiveRebinding()      //;
                                                                             //if (true){
                                                                             //      어쩌고
                                                                             //}
            //.WithTargetBinding(1)       // 오른쪽 왼쪽 어쩌고
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<keyboard>/escape")
            .OnComplete(op =>
        {
            Debug.Log(op.selectedControl.name);
            op.Dispose();
            var json = _inputAction.SaveBindingOverridesAsJson();
            Debug.Log(json);

            _inputAction.LoadBindingOverridesFromJson(json);    

            _inputAction.Player.Enable();
        })
        .OnCancel(op =>
        {
            Debug.Log("취소되었습니다.");
            op.Dispose();
            _inputAction.Player.Enable();
        })
        .Start();
    }
    
    private void UIPerformPress(InputAction.CallbackContext context)
    {
        Debug.Log("UI 상태");

    }

    private bool _uiMove = false;

    private void Update()
    {
        Vector2 inputDir = _inputAction.Player.Movement.ReadValue<Vector2>();
        OnMovement?.Invoke(inputDir);

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _inputAction.Disable();
            if (_uiMove == false)
            {
                _inputAction.UI.Enable();
            }
            else
            {
                _inputAction.Player.Enable();
            }

            _uiMove = !_uiMove;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }
}