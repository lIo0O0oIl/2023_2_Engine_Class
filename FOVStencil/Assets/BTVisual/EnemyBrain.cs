using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBrain : MonoBehaviour
{
    public Transform targetTrm;
    //기�? ?�등 브레?�에???�요??것들???�기 ?�어??
    public abstract void Attack();
}
