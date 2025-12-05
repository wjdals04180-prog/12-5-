using UnityEngine;

namespace AshleyR
{
    public class GearController : MonoBehaviour
    {
        //제어할 애니메이터 파라메터 이름
        public string parameterName = "RotateSpeed";
        [Range(0f, 100f)]
        public float speedValue = 0f; //회전속도
        public float direction = 1f;    //회전방향
        public float changeValue = 0.1f; //속도 변화값

        private Animator _animator;

        void Start()
        {
            //애니메이터를 제어하기 위해서 미리 캐싱(객체의 주소값을 기억시키는 일)함.
            _animator = GetComponent<Animator>();
            //기어 게임오브젝트 시작시 회전 속도 초기화.
            _animator.SetFloat(parameterName, direction * speedValue);
        }

        public void OnFaster()
        {
            //Mathf.Clamp 함수는 첫번째 파라메터의 값을 두번째(최소값), 세번째(최대값) 사이의 값으로 고정시키는 함수
            speedValue = Mathf.Clamp(speedValue + changeValue, 0f, 100f);
            //애니메이터에게 float형의 지정한 이름을 가진 파라미터에 값을 수정하는 요청할 수 있다.
            _animator.SetFloat(parameterName, direction * speedValue);
        }

        public void OnSlower()
        {
            speedValue = Mathf.Clamp(speedValue - changeValue, 0f, 100f);
            _animator.SetFloat(parameterName, direction * speedValue);
        }

        public void OnInvert()
        {
            //방향에다가 -1을 곱해서 방향을 반전시킨다.
            direction *= -1f;
            _animator.SetFloat(parameterName, direction * speedValue);
        }
    }

}
