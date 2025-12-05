using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    //AI의 행동 상태
    public enum MoveState
    {
        None = 0,
        Watching,
        Traceing,
        Returning,
        Max
    }
    [Header("감시 모드 관련")]
    //AI의 눈의 위치 정보
    public Transform eyePoint;
    [Range(10f, 200f)]
    //AI의 최대 시야거리
    public float watchableDistance = 10f;
    [Range(10f, 90f)]
    //AI의 최대 시야각
    public float watchableAngle = 30f;
    //감시모드시 최대 회전각
    public float watchRotateAngle = 15f;
    //감시모드시 회전 속도
    public float watchRotateSpeed = 20f;

    //씬 뷰에서 선택 상관없이 호출되어 필요한 정보를 화면에 그려줄 수 있다.
    private void OnDrawGizmos()
    {
        //시야 반경을 원형 그리기
        Handles.color = Color.red;
        Handles.DrawWireDisc(eyePoint.position, Vector3.up, watchableDistance);

        //부채꼴의 왼쪽/오른쪽 경계선을 계산
        Vector3 viewAngleLeft = CalculateAngle(-watchableAngle, false);
        Vector3 viewAngleRight = CalculateAngle(watchableAngle, false);
        //해서 그리기
        Handles.DrawLine(eyePoint.position, eyePoint.position + viewAngleLeft * watchableDistance);
        Handles.DrawLine(eyePoint.position, eyePoint.position + viewAngleRight * watchableDistance);

        //시야가 닿는 범위 그리기
        Handles.color = new Color(1f, 0f, 0f, 0.1f);
        //DrawSolidArc(중심점, 회전축, 시작 방향, 각도, 반지름);
        Handles.DrawSolidArc(eyePoint.position, Vector3.up, viewAngleLeft, watchableAngle * 2f, watchableDistance);

    }

    private Vector3 CalculateAngle(float angle, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angle += eyePoint.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    ////AI 에이전트
    //public NavMeshAgent agent;
    ////이동 목적지 위치정보
    //public Transform target;

    //public MoveState moveState = MoveState.Watching;
    //public Transform watchPoint;    

    //public float maxTraceDuration = 30f;
    //public float explosionDistance = 3f;

    ////AI 판단 딜레이 
    //public float interval = 1f;

    ////판단을 내려야 하는 시간
    //private float waitTime = 0f;


    //private float tracingExitTime = 0f;
    //private bool isLeft = false;


    //void Start()
    //{
    //    //시작시 AI 에이전트 미리 캐싱해놓는다.
    //    agent = GetComponent<NavMeshAgent>();
    //    SetWatchMode();
    //}

    //private void Update()
    //{
    //    //타겟없으면 목적지 설정안함.
    //    if (target == null)
    //        return;


    //    switch (moveState)
    //    {
    //        case MoveState.Watching:
    //            Watch();
    //            break;
    //        case MoveState.Return:
    //            Return();
    //            break;
    //        case MoveState.Tracing:
    //            Trace();
    //            break;
    //    }
    //}

    //private void Trace()
    //{
    //    if(tracingExitTime < Time.time)
    //    {
    //        SetReturnMode();
    //        return;
    //    }

    //    if(!CheckTarget(true, out float distance))
    //    {
    //        SetReturnMode();
    //        return;
    //    }

    //    if (distance < explosionDistance)
    //    {
    //        Explosion();
    //        return;
    //    }

    //    //판단 시간이 되지 않았으면 목적지 갱신 안함.
    //    if (waitTime > Time.time)
    //        return;



    //    //다음 AI판단 시간 갱신
    //    waitTime = Time.time + interval;

    //    //현재 목적지 갱신
    //    agent.SetDestination(target.position);
    //}

    //private void Watch()
    //{
    //    Quaternion destination = isLeft ? watchPoint.rotation * Quaternion.Euler(0f, -watchRotateAngle, 0f) :
    //        watchPoint.rotation * Quaternion.Euler(0f, watchRotateAngle, 0f);

    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, 
    //        destination, watchRotateSpeed * Time.deltaTime);

    //    if(Quaternion.Angle(destination, transform.rotation) < 0.001f)
    //        isLeft = !isLeft;

    //    if (CheckTarget(false, out float distance))
    //        SetTraceMode();
    //}

    //private void Return()
    //{
    //    if (CheckTarget(false, out float distance))
    //        SetTraceMode();

    //    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
    //        SetWatchMode();
    //}

    //private bool CheckTarget(bool onlyDistanceCheck, out float distance)
    //{
    //    Vector3 toward = target.position - eyePoint.position;
    //    distance = toward.magnitude;
    //    if (distance > watchableDistance)
    //        return false;

    //    if (onlyDistanceCheck)
    //        return true;

    //    if (Vector3.Dot(transform.forward, toward.normalized) < watchableAngle / 180f)
    //        return false;

    //    return true;
    //}

    //private void SetTraceMode()
    //{
    //    moveState = MoveState.Tracing;
    //    agent.updateRotation = true;
    //    agent.updatePosition = true;
    //    agent.SetDestination(target.position);
    //    waitTime = Time.time + interval;
    //    tracingExitTime = Time.time + maxTraceDuration;
    //}

    //private void SetWatchMode()
    //{
    //    isLeft = true;
    //    agent.updateRotation = false;
    //    agent.updatePosition = false;
    //    moveState = MoveState.Watching;
    //}

    //private void SetReturnMode()
    //{
    //    moveState = MoveState.Return;
    //    agent.SetDestination(watchPoint.position);
    //}

    //private void Explosion()
    //{
    //    target = null;
    //    Destroy(gameObject);
    //}

    //private void OnDrawGizmos()
    //{
    //    //1. 시야 반경 원 그리기
    //    Handles.color = Color.red;
    //    Handles.DrawWireDisc(trans)

    //}
}

