using UnityEngine;

public class MyEnemy : MonoBehaviour
{
    void Update()
    {
        // 여기엔 보통 에네미의 움직임(내려오기 등)을 넣습니다.
        // transform.Translate(Vector3.down * 1f * Time.deltaTime);
    }

    // [중요] 충돌함수는 Update 함수 밖에 작성해야 한다
    private void OnTriggerEnter2D(Collider2D other)
    {
        
            Debug.Log("총알에 맞았다!");
            
            //상대방(총알)을 파괴
            Destroy(other.gameObject);
            
            //나(에네미)를 파괴
            Destroy(gameObject);
    }
}