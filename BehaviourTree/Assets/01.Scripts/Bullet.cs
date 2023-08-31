using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 dir)
    {
        _rigid.AddForce(dir, ForceMode.Impulse);
        Destroy(gameObject, 2f);
    }
}
