using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3f; // 총알의 수명 (초)
    public GameObject dustParticlePrefab; // 먼지 파티클 효과 프리팹

    private void Start()
    {
        // 총알의 수명이 지나면 자동으로 파괴되도록 설정
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Bird bird = collision.gameObject.GetComponent<Bird>();
            if (bird != null)
            {
                bird.OnDeath(); // 적의 OnDeath() 메서드 호출
            }
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        // 총알 오브젝트 파괴
        Destroy(gameObject);

        // 먼지 파티클 효과 생성
        if (dustParticlePrefab != null)
        {
            Instantiate(dustParticlePrefab, transform.position, Quaternion.identity);
        }
    }
}