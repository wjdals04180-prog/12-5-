using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Clicker : MonoBehaviour
{
    //AI 에이전트 배열
    public NavMeshAgent[] agents;
    //클릭했을 때 감지할 수 있는 레이어
    public LayerMask mask;
    //시작시 자동 설정되는 목적지 위치정보
    public Transform firstDestination;

    private void Start()
    {
        //등록된 모든 AI에이전트들에게 자동으로 목적지들을 일괄 설정한다.
        foreach (var agent in agents)
        {
            //AI에게 목적지를 설정하는 함수
            //SetDestination(목적지 위치 값);
            agent.SetDestination(firstDestination.position);
        }
    }

    //Click 했을 때 호출되는 함수.
    public void OnClick()
    {
        //마우스의 현재 위치값을 가지고 카메라가 바라보는 위치를 추적할 수 있다.
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        //카메라가 바라보는 위치에 존재하는 3d 콜라이더를 감지해 감지한 게 있으면 true 반환,
        //out 키워드로 감지한 콜라이더 정보를 알 수 있다.
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, mask))
        {
            foreach (var agent in agents)
            {
                //AI에게 목적지를 설정하는 함수
                agent.SetDestination(hit.point);
            }
        }
    }
}
