using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Sprite idleSprite;  // 평상시 이미지 (Enemy_A)
    public Sprite hitSprite;   // 맞았을 때 이미지 (Enemy_A_Hit)
    
    private SpriteRenderer sr;

    private float delta;
    

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = idleSprite; // 처음엔 평상시 모습
    }

    void Update()
    {
        // 마우스 클릭 시 피격 효과 실행
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MouseButtonDown! = Hit!"); // 화면을 클릭했을 때 감지할 수 있는가?
            
            StopAllCoroutines(); // 이미 실행 중인 복귀 로직이 있다면 취소 (연타 대비)
            StartCoroutine(HitEffect()); 
        }

        delta += Time.deltaTime;
        if (delta > 0.1f) //만약 0.1초 이후가 된다면
        {
            delta = 0; //0.1초 후에 원하는 작업을 여기서 한다 (초기화함)
        }
    }

    // 시시오도시 로직: 맞았다가(상태 변화) 일정 시간 후 복귀
    IEnumerator HitEffect()
    {
        // 1. 이미지 교체 (대나무통이 기울어짐)
        sr.sprite = hitSprite;

        // 2. 0.1초 대기 (물이 쏟아지는 시간)
        yield return new WaitForSeconds(0.1f);

        // 3. 원래 이미지로 복구 (대나무통이 제자리로 돌아옴)
        sr.sprite = idleSprite;
    }
}