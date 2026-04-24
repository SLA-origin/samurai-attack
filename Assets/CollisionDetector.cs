using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
            // 적과 부딛혔는지 확인
            Debug.Log($"적 {other.gameObject.name}과 충돌!");

            // 상대방(적) 파괴하라
            Destroy(other.gameObject);

            // 중요: 가장 나중에 본인을 파괴하자
            // (정확히는 콜라이더가 설정돼 있고, Collision Detector 스크립트가 컴포넌트로 붙어 있는 자식 총알을 파괴한다)
            // 부모는 파괴되지 않으므로 그 안에 살아 남은 자식들은 계속 날아간다
            Destroy(gameObject);
        }
    
}