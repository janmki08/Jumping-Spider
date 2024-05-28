using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{
    public AudioClip flapSound; // 푸드덕 소리 클립
    private AudioSource flapAudioSource; // 오디오 소스 컴포넌트
    public AudioClip deathSound; // 푸드덕 소리 클립
    private AudioSource deathAudioSource; // 오디오 소스 컴포넌트

    public float maxDistance = 10f; // 소리가 들리는 최대 거리
    public float minVolume = 0f; // 최소 볼륨
    public float maxVolume = 1f; // 최대 볼륨

    private Transform playerTransform; // 플레이어의 Transform 컴포넌트
    public GameObject deathAnimationPrefab; // 죽는 모션 애니메이션 프리팹
    public GameObject deathParticlePrefab; // 새 파괴 파티클 효과 프리팹

    private static Bird closestBird; // 가장 가까운 적의 Bird 컴포넌트
    public GameObject itemPrefab; // 생성할 아이템 프리팹

    private void Start()
    {
        // 오디오 소스 컴포넌트 가져오기
        flapAudioSource = GetComponent<AudioSource>();
        deathAudioSource = GetComponent<AudioSource>();
        // 플레이어 오브젝트 찾기
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // 플레이어 오브젝트가 파괴되었는지 확인
        if (playerTransform == null)
        {
            // 플레이어 오브젝트가 파괴된 경우 스크립트 실행 중단
            return;
        }

        // 플레이어와의 거리 계산
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // 현재 적이 플레이어와 가장 가까운 적인지 확인
        if (closestBird == null || distance < Vector2.Distance(closestBird.transform.position, playerTransform.position))
        {
            closestBird = this;
        }

        // 가장 가까운 적의 소리만 재생
        if (closestBird == this)
        {
            // 거리에 따른 볼륨 계산
            float volume = Mathf.Clamp(1f - (distance / maxDistance), minVolume, maxVolume);
            flapAudioSource.volume = volume;
        }
        else
        {
            flapAudioSource.volume = 0f; // 다른 적들의 소리는 볼륨을 0으로 설정
        }
    }

    public void OnDeath()
    {
        // 충돌 비활성화
        GetComponent<Collider2D>().enabled = false;

        // 죽는 모션 애니메이션 생성
        if (deathAnimationPrefab != null)
        {
            GameObject deathAnimation = Instantiate(deathAnimationPrefab, transform.position, Quaternion.identity);

            // 적의 움직임 방향에 따라 죽는 모션 애니메이션 플립
            EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                SpriteRenderer deathAnimationRenderer = deathAnimation.GetComponent<SpriteRenderer>();
                if (deathAnimationRenderer != null)
                {
                    deathAnimationRenderer.flipX = !enemyMovement.movingRight;
                }
            }

            // 0.5초 후에 죽는 모션 애니메이션 프리팹 파괴
            Destroy(deathAnimation, 0.5f);
        }

        // 적 오브젝트 파괴
        Destroy(gameObject);

        // 아이템 생성
        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }
    }
}