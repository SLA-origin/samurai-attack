using UnityEngine;

public class Flappy : MonoBehaviour
{
    public float jumpForce = 5.0f;    // 위로 점프하는 힘
    private Rigidbody2D rb;          // 리짓바디 변수 선언
    private Animator anim;           // 애니메이션 제어 변수 선언

    void Start()
    {
        // 변수에 컴포넌트 연결하기
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 스페이스바를 누를 때
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 현재 떨어지고 있거나 올라가고 있는 속도를 순식간에 0으로 만들기
            // (그래야 누를 때마다 일정한 높이로 점프)
            rb.linearVelocity = Vector2.zero;

            // 리짓바디(물리엔진) 변수에 순간적인 힘을 가하고 점프시키기
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

            // 메카님 작동
            anim.SetTrigger("isFlapping");
        }
    }
}