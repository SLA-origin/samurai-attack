// using UnityEngine;
//
// public class Samurai : MonoBehaviour
// {
//     // 실행하면 Idle 애니메이션을 해라
//     // 키보드 왼쪽 또는 오른쪽 화살표를 누르고 있는 동안 Run 애니메이션을 해라
//     // 키보드를 누르고 있지 않다면 다시 Idle 애니메이션을 작동시켜라 
//         
//     public Sprite[] idleSprites;
//     public Sprite[] runSprites;
//     
//     private SpriteRenderer sr;
//     
//     private bool isRunning;
//     private float time;
//     private int dirX;
//     private int idx;
//     
//     
//     void Start()
//     {
//         Application.targetFrameRate = 60;
//         sr = GetComponent<SpriteRenderer>();
//     }
//     
//     void Update()
//     {
//         // 1. 입력 감지 및 방향/상태 설정
//         int dirX = (int)Input.GetAxisRaw("Horizontal");
//         isRunning = (dirX != 0); // 0이 아니면 달리는 중!
//     
//         if (isRunning) sr.flipX = (dirX == -1);
//
//         // 2. 타이머 로직
//         time += Time.deltaTime;
//         if (time >= 0.1f)
//         {
//             time = 0;
//             idx++;
//         }
//
//         // 3. 애니메이션 재생 (배열 길이 예방 로직 포함)
//         if (isRunning)
//         {
//             if (idx >= runSprites.Length) idx = 0; // Run 배열 길이에 맞춤
//             sr.sprite = runSprites[idx];
//         }
//         else
//         {
//             if (idx >= idleSprites.Length) idx = 0; // Idle 배열 길이에 맞춤
//             sr.sprite = idleSprites[idx];
//         }
//     }
// }
using UnityEngine;

public class Samurai_1 : MonoBehaviour
{
    public Sprite[] idleSprites;
    public Sprite[] runSprites;
    public float moveSpeed = 5.0f; // 이동 속도 추가
    
    private SpriteRenderer sr;
    private bool isRunning;
    private float time;
    private int idx;

    void Start()
    {
        Application.targetFrameRate = 60;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 1. 입력 감지
        float inputX = Input.GetAxisRaw("Horizontal"); // -1, 0, 1 반환
        
        // 2. 이동 및 방향 설정
        if (inputX != 0)
        {
            // 실제 위치 이동 (Time.deltaTime을 곱해 사양에 관계없이 일정하게 이동)
            transform.position += new Vector3(inputX * moveSpeed * Time.deltaTime, 0, 0);
            
            // 좌우 반전
            sr.flipX = (inputX == -1);

            // 상태가 Idle에서 Run으로 바뀔 때 idx 초기화
            if (!isRunning)
            {
                isRunning = true;
                idx = 0;
                time = 0;
            }
        }
        else
        {
            // 상태가 Run에서 Idle로 바뀔 때 idx 초기화
            if (isRunning)
            {
                isRunning = false;
                idx = 0;
                time = 0;
            }
        }

        // 3. 타이머 로직 (애니메이션 속도 조절)
        time += Time.deltaTime;
        if (time >= 0.1f)
        {
            time = 0;
            idx++;
        }

        // 4. 애니메이션 스프라이트 할당
        if (isRunning)
        {
            // % 연산자를 사용하면 if문 없이도 index 범위를 안전하게 순환시킬 수 있습니다.
            sr.sprite = runSprites[idx % runSprites.Length];
        }
        else
        {
            sr.sprite = idleSprites[idx % idleSprites.Length];
        }
    }
}