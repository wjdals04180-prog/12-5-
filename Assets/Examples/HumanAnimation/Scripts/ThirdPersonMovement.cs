using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    //카메라 타겟의 이름
    public string targetName = "CameraTarget";
    //카메라 타겟 위치정보
    public Transform cameraTarget;
    //애니메이터
    public Animator animator;
    //리지드바디
    public Rigidbody rb;
    //회전 속도
    public float rotateSpeed = 720f;
    //이동 전환 강도
    public float moveStrengh = 10f;

    //현재 이동 전환값
    private float _currentSpeed = 0f;
    //이동방향
    private Vector2 _direction;

    void Start()
    {
        //시작시 애니메이터를 미리 캐싱해 놓는다.
        animator  = GetComponent<Animator>();
        //시작시 리지드바디를 미리 캐싱해 놓는다.
        rb= GetComponent<Rigidbody>();
        //자식중에서 카메라타겟이름으로 되어 있는 자식을 찾아 캐싱한다.
        cameraTarget = transform.Find(targetName);
    }

    private void Update()
    {
        //현재 이동 강도를 서서히 상승, 혹은 하강하도록 인터폴레이션시킨다.
        _currentSpeed = Mathf.Lerp(_currentSpeed, _direction.magnitude, moveStrengh * Time.deltaTime);
        //현재 이동 강도가 0.01보다 클때만 이동상태로 만든다.
        bool isMoving = _currentSpeed > 0.01f;
        //현재 이동상태를 애니메이터에게 알린다.
        animator.SetBool("IsMoving", isMoving);
    }

    private void OnAnimatorMove()
    {
        //카메라가 바라보는 정면의 단위벡터값을 구하고
        Vector3 forward = cameraTarget.forward;
        //위아래 값은 무시한다.
        forward.y = 0;
        //y값을 무시하면서 달라진 벡터 길이를 다시 1로 바꿔 단위벡터로 변환시킨다.
        forward.Normalize();

        //카메라가 바라보는 오른쪽 방향의 단위 벡터값을 구하고
        Vector3 right = cameraTarget.right;
        //위아래 값은 무시한다.
        right.y = 0;
        //y값을 무시하면서 달라진 벡터 길이를 다시 1로 바꿔 단위벡터로 변환시킨다.
        right.Normalize();

        //만약 키보드WASD조작을 하고 있다면
        if(_direction != Vector2.zero)
        {
            //정면방향과 우측방향에 키보드 방향조작을 곱해 이동해야할 방향을 알아낸다.
            Vector3 moveDirection = forward * _direction.y + right * _direction.x;
            //이동방향을 향해 자연스럽게 서서히 회전하도록 회전시킨다.
            //RotateToward(현재 회전값, 목표 회전값, 최대 회전가능한 각도)
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(moveDirection),
                rotateSpeed * Time.deltaTime);
        }
       
        //애니메이션 동작의 실제 이동거리를 알아내 초당 이동 거리 및 방향을 알아내고
        Vector3 velocity = animator.deltaPosition / Time.deltaTime;
        //중력을 적용한 후
        velocity.y = rb.linearVelocity.y;
        //리지드바디에 속도를 입력한다.
        rb.linearVelocity = velocity;
    }

    //키보드WASD 입력할 때 호출되어 입력값을 확인할 수 있는 함수
    public void OnMove(InputValue value)
    {
        _direction = value.Get<Vector2>();
    }
}
