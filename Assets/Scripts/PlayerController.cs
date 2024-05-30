using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public GrapplingHook grappling;
    private Rigidbody2D rb;
    public float moveInput = 0f;
    private float speed = 8f;
    private Collider2D col;
    private SpriteRenderer sprite;
    public Sprite idle;
    public Sprite jumping;
    public Sprite landing;
    public float pendulumForce = 5f; // 진자 운동 힘 조절 변수

    public AudioClip swingSound; // 웹스윙 사운드 클립
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    private GameOver gameOver; // GameOverRestartUI 스크립트 참조
    private Restart restartScript; // Restart 스크립트 참조

    public float itemEffectDuration = 2f; // 아이템 효과 지속 시간
    public float itemUpwardForce = 10f; // 아이템 효과로 인한 위쪽 방향 힘
    public float invincibilityDuration = 3f; // 무적 효과 지속 시간

    private bool isItemEffectActive = false; // 아이템 효과 활성화 여부
    private bool isInvincible = false; // 무적 상태 여부

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        grappling = GetComponent<GrapplingHook>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();

        // GameOver 스크립트 찾기
        gameOver = FindObjectOfType<GameOver>();
        restartScript = FindObjectOfType<Restart>();
    }

    void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (grappling.isAttach)
        {
            // 그래플링 중 충돌 제거
            col.enabled = false;

            Vector2 hookposition = grappling.GetHookPosition();
            Vector2 pendulumDirection = (hookposition - (Vector2)transform.position).normalized;

            float pendulumSpeed = Vector2.Dot(rb.velocity, pendulumDirection);

            // 진자 운동 효과 적용
            rb.AddForce(-pendulumDirection * pendulumSpeed * pendulumForce);

            // 좌우 이동 입력 적용
            Vector2 perpendicularDirection = -Vector2.Perpendicular(pendulumDirection);
            rb.AddForce(perpendicularDirection * moveInput * speed);

            // 웹 스윙 사운드 재생
            if (!audioSource.isPlaying)
            {
                audioSource.clip = swingSound;
                audioSource.Play();
            }
        }
        else
        {
            // 그래플링 상태가 아닐 때 충돌 활성화
            col.enabled = true;

            // 관성 적용
            if (grappling.isReleased)
            {
                rb.velocity = grappling.releaseVelocity;
                grappling.isReleased = false;
            }
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

            // 웹 스윙 사운드 중지
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        if (rb.velocity.y > 0)
        {
            sprite.sprite = jumping;
        }
        else if (rb.velocity.y < 0)
        {
            sprite.sprite = landing;
        }
        else if (rb.velocity.y == 0)
        {
            sprite.sprite = idle;
        }
    }

    public bool IsGrappling
    {
        get
        {
            return grappling.isAttach;
        }
    }

    // 리스타트
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                // 무적 상태가 아닐 때만 게임 오버 처리
                if (gameOver != null)
                {
                    gameOver.OnPlayerCollideWithEnemy();
                    restartScript.OnPlayerCollideWithEnemy();
                }
            }
        }
    }

    public void ApplyItemEffect()
    {
        if (!isItemEffectActive)
        {
            // 아이템 효과 활성화
            isItemEffectActive = true;
            Invoke("DeactivateItemEffect", itemEffectDuration);

            // 위쪽 방향으로 힘 적용
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * itemUpwardForce, ForceMode2D.Impulse);

            // 무적 효과 활성화
            StartCoroutine(InvincibilityEffect());
        }
    }

    private void DeactivateItemEffect()
    {
        // 아이템 효과 비활성화
        isItemEffectActive = false;
    }

    private IEnumerator InvincibilityEffect()
    {
        isInvincible = true;
        // 무적 상태일 때의 시각적 효과 (예: 플레이어 깜빡임)
        for (float t = 0; t < invincibilityDuration; t += 0.1f)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        sprite.enabled = true;
        isInvincible = false;
    }
    public bool IsInvincible
    {
        get { return isInvincible; }
    }
}