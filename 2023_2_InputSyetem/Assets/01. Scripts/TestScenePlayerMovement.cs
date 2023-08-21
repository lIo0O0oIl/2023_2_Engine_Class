using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TestScenePlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _gravityMultiplier = 1f;
    [SerializeField] private float _jumpPower = 3f;

    private CharacterController _characterController;
    public bool IsGround => _characterController.isGrounded;

    private Vector2 _inputDirection;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity;
    private float _verticalVelocity;

    public bool ActiveMove { get; set; } = true;

    private TestScenePlayerInput _playerInput;

    [SerializeField] private LayerMask _Ground;

    [SerializeField] private Transform _visual;
    [SerializeField] private GameObject fireBallPrefabs;
    private Camera _camera;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<TestScenePlayerInput>();
        _playerInput.OnMovement += SetPlayerMovement;
        _playerInput.OnJump += Jump;
    }

    private void Jump()
    {
        if (!IsGround) return; //땅에 착지 상태가 아니면 리턴
        _verticalVelocity += _jumpPower;
    }

    private void SetPlayerMovement(Vector2 dir)
    {
        _inputDirection = dir;
    }

    private void CalculateMovement()
    {
        _movementVelocity = new Vector3(_inputDirection.x, 0, _inputDirection.y)
                            * (_moveSpeed * Time.fixedDeltaTime);

        if (_movementVelocity.sqrMagnitude > 0)
        {
            //transform.rotation = Quaternion.LookRotation(_movementVelocity);
        }
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
        _movementVelocity.y = _verticalVelocity;
    }

    private void Move()
    {
        _characterController.Move(_movementVelocity);
    }

    private void FixedUpdate()
    {
        if (ActiveMove)
        {
            CalculateMovement();
        }
        ApplyGravity();
        Move();
    }

    //요건 나중에 쓸 함수들
    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }

    //이건 나중에 넉백 구현할 일 있으면 쓰면 된다.
    public void SetMovement(Vector3 value)
    {
        _verticalVelocity = value.y;
        _movementVelocity = new Vector3(value.x, 0, value.z);
    }



    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        Vector2 mousePos = _playerInput.aimPosition;
        Ray ray = _camera.ScreenPointToRay(mousePos);
        //Debug.Log(ray.direction);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _camera.farClipPlane, _Ground))
        {
            Vector3 worldMousePos = hitInfo.point;
            Vector3 dir = (worldMousePos - transform.position);
            dir.y = 0;
            _visual.rotation = Quaternion.LookRotation(dir);
        }
    }
}