using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _parentTrm;
    [SerializeField] private Transform _gunNozzle;
    [SerializeField] private CinemachineFreeLook _freeLookCam;

    private PlayerMovement _movement;
    private CinemachineImpulseSource _impulseSource;

    private bool _isPressFire = false;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _inputReader.FireEvent += OnHandleFire;
    }

    private void Update()
    {
        Vector3 angle = _parentTrm.localEulerAngles;
        _movement.blockRotationPlayer = _isPressFire;       // ÃÑ¾Ë ¹ß»ç¸ðµå ÀÏ ¶§ ¸ØÃã

        if (_isPressFire)
        {
            VisualPolish();
            _movement.RotateToCamera(transform);
        }

        var camYAxis = 0f;
        if (_isPressFire)
        {
            camYAxis = RemapCamera(_freeLookCam.m_YAxis.Value, 0, 1, -25, 25);
        }

        _parentTrm.localEulerAngles = new Vector3(Mathf.LerpAngle(_parentTrm.localEulerAngles.x, camYAxis, 0.3f), angle.y, angle.z);
    }

    private void VisualPolish()
    {
        if (!DOTween.IsTweening(_parentTrm))
        {
            _parentTrm.DOComplete();
            Vector3 forward = _parentTrm.forward;
            Vector3 localPos = _parentTrm.localPosition;

            _parentTrm.DOLocalMove(localPos - new Vector3(0, 0, 0.2f), 0.03f)
                .OnComplete(() => _parentTrm.DOLocalMove(localPos, 0.1f).SetEase(Ease.OutSine));

            _impulseSource.GenerateImpulse(0.2f);
        }

        if (!DOTween.IsTweening(_gunNozzle))
        {
            _gunNozzle.DOComplete();
            _gunNozzle.DOPunchScale(new Vector3(0, 1, 1) / 1.5f, 0.15f, 10, 1);
        }
    }

    private void OnHandleFire(bool value)
    {
        _isPressFire = value;
    }

    private float RemapCamera(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private void OnDestroy()
    {
        _inputReader.FireEvent -= OnHandleFire;
    }
}
