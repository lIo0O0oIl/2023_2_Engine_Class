using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerFOV))]
public class FOVEditor : Editor
{
    // ���� �׷��� �� �����ϴ� �Լ�
    private void OnSceneGUI()
    {
        var pFov = target as PlayerFOV;
        var pos = pFov.transform.position;
        Handles.color = Color.white;
        Handles.DrawWireArc(pos, Vector3.up, Vector3.forward, 360f, pFov.viewRadius);

        Vector3 viewAngleA = pFov.DirFromAngle(-pFov.viewAngle * 0.5f, false);
        Vector3 viewAngleB = pFov.DirFromAngle(pFov.viewAngle * 0.5f, false);

        Handles.DrawLine(pos, pos + viewAngleA * pFov.viewRadius);
        Handles.DrawLine(pos, pos + viewAngleB * pFov.viewRadius);
    }
}
