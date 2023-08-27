using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private TestSceneFireBall _ballPrefab;
    [SerializeField] private Transform _firePos;

    private TestScenePlayerInput _inputReader;

    private void Awake()
    {
        _inputReader = GetComponent<TestScenePlayerInput>();
        _inputReader.OnFire += FireHandle;
    }

    private void FireHandle()
    {
        //TestSceneFireBall ball = Instantiate(_ballPrefab, _firePos.position, Quaternion.identity);
        TestSceneFireBall ball = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
        //Debug.Log(_firePos.forward);
        ball.Fire(_firePos.forward * 20f);
    }
}