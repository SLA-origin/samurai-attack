using UnityEngine;

public class MyPlayerBullet : MonoBehaviour
{
    public float speed = 1f; // 속도를 조절할 수 있게 변수로 빼서 선언하자

    void Update()
    {
        // 매 프레임마다 이동하라 : Vector3.up (아래에서 위로) 방향으로 이동하라
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}