using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBrain : MonoBehaviour
{
    public Transform targetTrm;
    //ê¸°í? ?±ë“± ë¸Œë ˆ?¸ì—???„ìš”??ê²ƒë“¤???¬ê¸° ?£ì–´??
    public abstract void Attack();
}
