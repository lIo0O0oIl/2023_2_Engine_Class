using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("����������")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravity = -9.8f;

    [Header("���ð���")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _rootTrm;
    [SerializeField] private float _gravityMultiplier = 1f;

    private CharacterController _characterController;
    public bool IsGround => _characterController.isGrounded;

    private Vector2 _inputDirection;

    public Vector3 MovementVelocity { get; private set; }
    private float _verticalVelocity;

    public bool ActiveMove { get; private set; } = true;        // �⺻������ ��Ƽ�� ����

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputReader.Movement += SetMovement;
    }

    private void OnDisable()
    {
        _inputReader.Movement -= SetMovement;
    }

    private void SetMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    private void CalcuateMovement()
    {
        MovementVelocity = (_rootTrm.forward * _inputDirection.y + _rootTrm.right * _inputDirection.x)
            * (+_moveSpeed * Time.fixedDeltaTime);      // Ű���� �̵�, ���콺�� ���� �̵��̱� ������
    }

    public void StopImmediately()
    {
        MovementVelocity = Vector3.zero;
    }

    public void SetMovementVelocity(Vector3 value)
    {
        MovementVelocity = new Vector3(value.x, 0, value.y);
        _verticalVelocity = value.y;
    }

    private void ApplyGravity()
    {
        if (IsGround && _verticalVelocity < 0)
        {
            _verticalVelocity = -1f;
        }
        else
        {
            _verticalVelocity += _gravity * _gravityMultiplier * Time.fixedDeltaTime;
        }
        Vector3 value = MovementVelocity;
        value.y = _verticalVelocity;
        MovementVelocity = value;
    }

    private void Move()
    {
        _characterController.Move(MovementVelocity);
    }

    private void FixedUpdate()
    {
        if (ActiveMove)
        {
            CalcuateMovement();
        }
        ApplyGravity();
        Move();
    }
}