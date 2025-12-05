using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    //카메라 타겟
    public Transform target;
    //마우스 감도
    public float mouseSensitivity = 5f;
    //마우스의 위치 변화값
    private Vector2 _delta;
    //카메라의 X축 현재 회전 방향
    private float _pitch = 0f;
    //카메라의 Y축 현재 회전 방향
    private float _yaw = 0f;

    void Start()
    {
        //활성화시 현재 카메라의 방향값을 저장해둔다.
        Vector3 rotation = target.rotation.eulerAngles;
        _pitch = rotation.y;
        _yaw = rotation.x;
    }

    void LateUpdate()
    {
        //마우스의 Y축 변화량을 카메라 X축 회전에 적용한다.
        _pitch -= _delta.y * mouseSensitivity * Time.deltaTime;
        //위, 아래 90도 이상 회전하지 못하게 막는다.(이유 : X축 회전을 막지 않으면 카메라의 위아래가 반전되기 때문에)
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        //마우스의 X축 변화량을 카메라 Y축 회전에 적용한다.
        _yaw += _delta.x * mouseSensitivity * Time.deltaTime;

        //카메라 타겟에 회전값을 적용한다. 
        //컴퓨터는 사원수만 알고 있으므로 360도 개념을 사원수로 변환해
        //적용해야 해서 Quaternion.Euler함수로 변환해서 적용한다.
        target.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    //마우스 포인터의 위치변화값을 알려주는 함수
    public void OnLook(InputValue value)
    {
        //변화값을 변수에 대입한다.
        _delta = value.Get<Vector2>();
    }
}
