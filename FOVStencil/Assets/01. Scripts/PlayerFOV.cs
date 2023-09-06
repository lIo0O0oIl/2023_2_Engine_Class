using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 플레이어 FO 뷰?
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
    [SerializeField] private float _meshResolution;     // 얼마나 촘촘하게 그릴꺼니
    [SerializeField] private int _edgeResolveIteration;     // 2분 탐색을 몇 번 할꺼니?
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
            FindVisibleEnemy();     // 0.3 초마다 에너미를 찾을꺼야
        }
    }

    private void FindVisibleEnemy()
    {
        visibleTargets.Clear();
        Collider[] enemiesInView = new Collider[6];
        int cnt = Physics.OverlapSphereNonAlloc(transform.position, viewRadius, enemiesInView, _enemyMask);
        // 그냥 오버랩쓰면 힙에다 둬서 성능저하를 일으켜서 유니티에서 이걸 쓰라고 하는 것임. 그냥 내가 지정해준 배열만큼만 넣어주겠다.

        for (int i = 0; i < cnt; i++)       // 시야각에 있는거만 가져오는 것
        {
            Transform enemy = enemiesInView[i].transform;
            Vector3 dir = enemy.position - transform.position;
            Vector3 dirToEnemy = dir.normalized;

            if (Vector3.Angle(transform.forward, dirToEnemy) < viewAngle * 0.5f)
            {
                // 시야 범위안에 있다. 그럼 그녀석에세 Ray 를 쏴서 그안에 장애물도 없다는 것을 알아내야해.
                // 레이를 쏴서 만약 아무것도 없다면 visibleTargets.Add(enemy);
                if (!Physics.Raycast(transform.position, dirToEnemy, dir.magnitude, _obstacleMask))      // 뷰레디오스로 하면 안됨.
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

        // 2분 탐색
        for (int i = 0; i < _edgeResolveIteration; i++)
        {
            float angle = (minAngle + maxAngle) * 0.5f;
            var castInfo = ViewCast(angle);     // 새로운 중간각으로 캐스트 시작

            bool thresholdExceeded = Mathf.Abs(minViewCast.distance - castInfo.distance) > _edgeDistanceThreshold;
            // 쏜 것의 격차가 뜨레스홀드를 넘어섰는지

            // 두개의 레이가 모두 피격상태가 같고 거리도 일정 
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
        int stepCount = Mathf.RoundToInt(viewAngle * _meshResolution);      // 촘촘 할수록 메쉬가 늘어나 부드러워짐, 그러나 성능저하가 올 수도 있음
        float stepAngleSize = viewAngle / stepCount;        // 각도 구함

        Vector3 pos = transform.position;
        List<Vector3> viewPoints = new List<Vector3>();

        var oldViewCastInfo = new ViewCastInfo();       // 예전 정보를 기억할 변수

        for (int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle * 0.5f + stepAngleSize * i;
            // 현재 돌아가있는 만큼을 돌려서 계산한다.
            //Debug.DrawLine(pos, pos + DirFromAngle(angle, true) * viewRadius, Color.red);
            // 월드자표로 레이캐스트가 사용됨.

            var info = ViewCast(angle);

            if (i > 0)      // 첫번쨰 캐스트가 아니라면, 이전 캐스트 정보가 저장되어 있겠지
            {
                bool thresholdExceeded = Mathf.Abs(oldViewCastInfo.distance - info.distance) > _edgeDistanceThreshold;
                // 이전과 지금에서 달라진 거다 

                if (oldViewCastInfo.hit != info.hit || (oldViewCastInfo.hit && info.hit && thresholdExceeded))      // 피격이 다르거나 둘다 피격인데 뭐가 있다면
                {
                    var edge = FindEdge(oldViewCastInfo, info);

                    if (edge.pointA != Vector3.zero)        // 민포인터는 저장할 필요가 없다...? 네? 암튼 부르럽게 연결하기 위해서임
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
            oldViewCastInfo = info;     // 이전 정보에 현재 정보 넣어줌
        }

        int vertCount = viewPoints.Count + 1;       // 버택스하나가 더 필요함
        Vector3[] vertices = new Vector3[vertCount];
        int[] triangles = new int[(vertCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);       // 로컬좌표로 바꿔줌

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
        _viewMesh.RecalculateNormals(); // 노말 재계산

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
