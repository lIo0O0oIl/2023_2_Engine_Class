using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugManager;

public class TestScenePlayerInput : MonoBehaviour
{
    private PlayerInputAction _inputAction;
    public PlayerInputAction InputAction => _inputAction;

    public event Action<Vector2> OnMovement;
    public event Action<Vector2> OnAim;     // 마우스 스크린
    public event Action OnJump;
    public event Action OnFire;

    public Vector2 aimPosition;

    [SerializeField] private GameObject _uiDocument;

    private void Awake()
    {
        _inputAction = new PlayerInputAction();

        _inputAction.Player.Enable();
        _inputAction.Player.Jump.performed += JumpHandle;
        _inputAction.Player.Fire.performed += FireHandle;

        _inputAction.UI.Submit.performed += UIPerformPress;
    }

    private void UIPerformPress(InputAction.CallbackContext context)
    {
        Debug.Log("UI 상태");
    }

    private void FireHandle(InputAction.CallbackContext context)
    {
        OnFire?.Invoke();
    }

    private void JumpHandle(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }

    private bool _uiMove = false;

    private void Update()
    {
        aimPosition = _inputAction.Player.Aim.ReadValue<Vector2>();

        Vector2 inputDir = _inputAction.Player.Movement.ReadValue<Vector2>();
        OnMovement?.Invoke(inputDir);

        /*if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _inputAction.Disable();
            if (_uiMove == false)
            {
                _uiDocument.SetActive(true);
                _inputAction.UI.Enable();
            }
            else
            {
                _uiDocument.SetActive(false);
                _inputAction.Player.Enable();
            }

            _uiMove = !_uiMove;
        }*/
    }
}