using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �÷��̾� FO ��?
public struct ViewCastInfo
{
    public bool hit;
    public Vector3 point;
    public float distance;
    public float angle;
}

public struct EdgeInfo
{
    public Vector3 pointA;
    public Vector3 pointB;
}

public class PlayerFOV : MonoBehaviour
{
    [Range(0, 360)] public float viewAngle;
    public float viewRadius;

    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _enemyFindDelay = 0.3f;
    [SerializeField] private float _meshResolution;     // �󸶳� �����ϰ� �׸�����
    [SerializeField] private int _edgeResolveIteration;     // 2�� Ž���� �� �� �Ҳ���?
    [SerializeField] private float _edgeDistanceThreshold;

    public List<Transform> visibleTargets = new List<Transform>();

    private MeshFilter _viewMeshFilter;
    private Mesh _viewMesh;

    private void Awake()
    {
        _viewMeshFilter = transform.Find("ViewVisual").GetComponent<MeshFilter>();
        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        _viewMeshFilter.mesh = _viewMesh;
    }

    public Vector3 DirFromAngle(float degree, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            degree += transform.eulerAngles.y;
        }
        float rad = degree * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }

    public void Start()
    {
        StartCoroutine(FindEnemyWithDelay(_enemyFindDelay));
    }

    IEnumerator FindEnemyWithDelay(float delay)
    {
        var time = new WaitForSeconds(delay);
        while (true)
        {
            yield return time;
            FindVisibleEnemy();     // 0.3 �ʸ��� ���ʹ̸� ã������
        }
    }

    private void FindVisibleEnemy()
    {
        visibleTargets.Clear();
        Collider[] enemiesInView = new Collider[6];
        int cnt = Physics.OverlapSphereNonAlloc(transform.position, viewRadius, enemiesInView, _enemyMask);
        // �׳� ���������� ������ �ּ� �������ϸ� �����Ѽ� ����Ƽ���� �̰� ����� �ϴ� ����. �׳� ���� �������� �迭��ŭ�� �־��ְڴ�.

        for (int i = 0; i < cnt; i++)       // �þ߰��� �ִ°Ÿ� �������� ��
        {
            Transform enemy = enemiesInView[i].transform;
            Vector3 dir = enemy.position - transform.position;
            Vector3 dirToEnemy = dir.normalized;

            if (Vector3.Angle(transform.forward, dirToEnemy) < viewAngle * 0.5f)
            {
                // �þ� �����ȿ� �ִ�. �׷� �׳༮���� Ray �� ���� �׾ȿ� ��ֹ��� ���ٴ� ���� �˾Ƴ�����.
                // ���̸� ���� ���� �ƹ��͵� ���ٸ� visibleTargets.Add(enemy);
                if (!Physics.Raycast(transform.position, dirToEnemy, dir.magnitude, _obstacleMask))      // �䷹������� �ϸ� �ȵ�.
                {
                    visibleTargets.Add(enemy);
                }
            }
        }
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;

        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        // 2�� Ž��
        for (int i = 0; i < _edgeResolveIteration; i++)
        {
            float angle = (minAngle + maxAngle) * 0.5f;
            var castInfo = ViewCast(angle);     // ���ο� �߰������� ĳ��Ʈ ����

            bool thresholdExceeded = Mathf.Abs(minViewCast.distance - castInfo.distance) > _edgeDistanceThreshold;
            // �� ���� ������ �߷���Ȧ�带 �Ѿ����

            // �ΰ��� ���̰� ��� �ǰݻ��°� ���� �Ÿ��� ���� 
            if (castInfo.hit == minViewCast.hit && !thresholdExceeded)
            {
                minAngle = angle;
                minPoint = castInfo.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = castInfo.point;
            }
        }
        return new EdgeInfo { pointA = minPoint, pointB = maxPoint };
    }

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * _meshResolution);      // ���� �Ҽ��� �޽��� �þ �ε巯����, �׷��� �������ϰ� �� ���� ����
        float stepAngleSize = viewAngle / stepCount;        // ���� ����

        Vector3 pos = transform.position;
        List<Vector3> viewPoints = new List<Vector3>();

        var oldViewCastInfo = new ViewCastInfo();       // ���� ������ ����� ����

        for (int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle * 0.5f + stepAngleSize * i;
            // ���� ���ư��ִ� ��ŭ�� ������ ����Ѵ�.
            //Debug.DrawLine(pos, pos + DirFromAngle(angle, true) * viewRadius, Color.red);
            // ������ǥ�� ����ĳ��Ʈ�� ����.

            var info = ViewCast(angle);

            if (i > 0)      // ù���� ĳ��Ʈ�� �ƴ϶��, ���� ĳ��Ʈ ������ ����Ǿ� �ְ���
            {
                bool thresholdExceeded = Mathf.Abs(oldViewCastInfo.distance - info.distance) > _edgeDistanceThreshold;
                // ������ ���ݿ��� �޶��� �Ŵ� 

                if (oldViewCastInfo.hit != info.hit || (oldViewCastInfo.hit && info.hit && thresholdExceeded))      // �ǰ��� �ٸ��ų� �Ѵ� �ǰ��ε� ���� �ִٸ�
                {
                    var edge = FindEdge(oldViewCastInfo, info);

                    if (edge.pointA != Vector3.zero)        // �������ʹ� ������ �ʿ䰡 ����...? ��? ��ư �θ����� �����ϱ� ���ؼ���
                    {
                        viewPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(info.point);
            oldViewCastInfo = info;     // ���� ������ ���� ���� �־���
        }

        int vertCount = viewPoints.Count + 1;       // ���ý��ϳ��� �� �ʿ���
        Vector3[] vertices = new Vector3[vertCount];
        int[] triangles = new int[(vertCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);       // ������ǥ�� �ٲ���

            if (i < vertCount - 2)
            {
                int tIndex = i * 3;
                triangles[tIndex + 0] = 0;
                triangles[tIndex + 1] = i + 1;
                triangles[tIndex + 2] = i + 2;
            }
        }
        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals(); // �븻 ����

    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, viewRadius, _obstacleMask))
        {
            return new ViewCastInfo
            {
                hit = true,
                point = hit.point,
                distance = hit.distance,
                angle = globalAngle
            };
        }
        else
        {
            return new ViewCastInfo
            {
                hit = false,
                point = transform.position + dir * viewRadius,
                distance = viewRadius,
                angle = globalAngle 
            };
        }
    }
}
