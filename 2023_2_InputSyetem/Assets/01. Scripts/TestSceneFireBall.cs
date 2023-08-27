using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneFireBall : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 dir)
    {
        _rigidbody.AddForce(dir, ForceMode.Impulse);
        // 순간적인 힘을 물체에 즉시 적용시켜 가속함
        //Destroy(gameObject, 2f);
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //    Destroy(gameObject);
    //}
}
